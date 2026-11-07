document.addEventListener("DOMContentLoaded", function () {
    // 1. Inicializar los detalles con los datos existentes de la factura
    let detalles = (typeof detallesIniciales !== "undefined" && Array.isArray(detallesIniciales))
        ? [...detallesIniciales]
        : [];

    const selectProducto = document.getElementById("selectProducto");
    const txtExistencia = document.getElementById("txtExistencia");
    const txtPrecio = document.getElementById("txtPrecio");
    const inputCantidad = document.getElementById("inputCantidad");
    const btnAgregarLinea = document.getElementById("btnAgregarLinea");
    const errorDetalle = document.getElementById("errorDetalle");
    const inputDescuento = document.getElementById("inputDescuento");
    const cuerpoTabla = document.getElementById("cuerpoTablaDetalles");
    const formFactura = document.getElementById("formFactura");

    // Al cambiar de producto, autocompleta existencia y precio unitario
    if (selectProducto) {
        selectProducto.addEventListener("change", function () {
            mostrarErrorDetalle("");

            const valorSeleccionado = this.value.toLowerCase();
            const prod = (typeof productosCatalogo !== "undefined")
                ? productosCatalogo.find(p => (p.Id || p.id).toLowerCase() === valorSeleccionado)
                : null;

            if (prod) {
                const existencia = prod.ExistenciaActual ?? prod.existenciaActual ?? 0;
                const precio = prod.PrecioPorUnidadMedida ?? prod.precioPorUnidadMedida ?? 0;

                txtExistencia.value = existencia;
                txtPrecio.value = "₡" + Number(precio).toLocaleString('es-CR', { minimumFractionDigits: 2 });
                inputCantidad.max = existencia;
                inputCantidad.value = "";
                inputCantidad.focus();
            } else {
                txtExistencia.value = "";
                txtPrecio.value = "";
                inputCantidad.value = "";
            }
        });
    }

    // Agregar línea de detalle validando stock
    if (btnAgregarLinea) {
        btnAgregarLinea.addEventListener("click", function () {
            mostrarErrorDetalle("");
            const prodId = selectProducto.value;
            const cantidad = parseFloat(inputCantidad.value);

            if (!prodId) {
                mostrarErrorDetalle("Debe seleccionar un producto del catálogo.");
                return;
            }

            const prod = (typeof productosCatalogo !== "undefined")
                ? productosCatalogo.find(p => (p.Id || p.id).toLowerCase() === prodId.toLowerCase())
                : null;

            if (!prod) return;

            const existencia = prod.ExistenciaActual ?? prod.existenciaActual ?? 0;
            const precio = prod.PrecioPorUnidadMedida ?? prod.precioPorUnidadMedida ?? 0;
            const codigo = prod.CodigoProducto ?? prod.codigoProducto ?? "";
            const nombre = prod.NombreProducto ?? prod.nombreProducto ?? "";
            const unidad = prod.UnidadMedida ?? prod.unidadMedida ?? 1;

            if (isNaN(cantidad) || cantidad <= 0) {
                mostrarErrorDetalle("La cantidad ingresada debe ser un número mayor a cero.");
                return;
            }

            if (cantidad > existencia) {
                mostrarErrorDetalle(`La cantidad supera la existencia actual disponible (${existencia}).`);
                return;
            }

            // Si el producto ya estaba en la lista, acumulamos cantidad
            const detalleExistente = detalles.find(d => (d.productoId || d.ProductoId).toLowerCase() === prodId.toLowerCase());
            if (detalleExistente) {
                if ((detalleExistente.cantidad + cantidad) > existencia) {
                    mostrarErrorDetalle(`No puede agregar esa cantidad. La existencia total disponible es ${existencia}.`);
                    return;
                }
                detalleExistente.cantidad += cantidad;
                detalleExistente.subtotal = detalleExistente.cantidad * detalleExistente.precioUnitario;
            } else {
                detalles.push({
                    productoId: prod.Id || prod.id,
                    codigo: codigo,
                    nombre: nombre,
                    unidadMedida: unidad,
                    cantidad: cantidad,
                    precioUnitario: precio,
                    subtotal: cantidad * precio
                });
            }

            // Limpiar controles temporales
            selectProducto.value = "";
            txtExistencia.value = "";
            txtPrecio.value = "";
            inputCantidad.value = "";

            renderizarTabla();
        });
    }

    // Regla de negocio: Descuento configurable (Máx. 25%)
    if (inputDescuento) {
        inputDescuento.addEventListener("input", function () {
            let val = parseFloat(this.value);
            if (isNaN(val) || val < 0) this.value = 0;
            if (val > 25) this.value = 25;
            calcularTotales();
        });
    }

    // Renderizar tabla con índices Detalles[i] para el Model Binder
    function renderizarTabla() {
        if (!cuerpoTabla) return;
        cuerpoTabla.innerHTML = "";

        if (detalles.length === 0) {
            cuerpoTabla.innerHTML = `<tr><td colspan="7" class="text-center text-muted">No se han agregado productos a la factura.</td></tr>`;
            calcularTotales();
            return;
        }

        detalles.forEach((det, i) => {
            const prodId = det.productoId || det.ProductoId;
            const cod = det.codigo || det.CodigoProducto;
            const nom = det.nombre || det.NombreProducto;
            const um = det.unidadMedida ?? det.UnidadMedida;
            const cant = parseFloat(det.cantidad ?? det.Cantidad);
            const precio = parseFloat(det.precioUnitario ?? det.PrecioUnitario);
            const sub = cant * precio;

            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>
                    <input type="hidden" name="Detalles[${i}].ProductoId" value="${prodId}" />
                    <input type="hidden" name="Detalles[${i}].CodigoProducto" value="${cod}" />
                    <input type="hidden" name="Detalles[${i}].NombreProducto" value="${nom}" />
                    <input type="hidden" name="Detalles[${i}].UnidadMedida" value="${um}" />
                    <input type="hidden" name="Detalles[${i}].Cantidad" value="${cant}" />
                    <input type="hidden" name="Detalles[${i}].PrecioUnitario" value="${precio}" />
                    ${cod}
                </td>
                <td>${nom}</td>
                <td>${um}</td>
                <td class="text-end">${cant.toFixed(2)}</td>
                <td class="text-end">₡${Number(precio).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                <td class="text-end">₡${Number(sub).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-icon btn-delete btn-quitar-detalle" data-indice="${i}" title="Quitar">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            `;
            cuerpoTabla.appendChild(tr);
        });

        // Eventos a botones de eliminar
        document.querySelectorAll(".btn-quitar-detalle").forEach(btn => {
            btn.addEventListener("click", function () {
                const idx = parseInt(this.getAttribute("data-indice"));
                detalles.splice(idx, 1);
                renderizarTabla();
            });
        });

        calcularTotales();
    }

    // Fórmulas de totales financieros
    function calcularTotales() {
        const subtotal = detalles.reduce((acc, curr) => {
            const cant = parseFloat(curr.cantidad ?? curr.Cantidad);
            const precio = parseFloat(curr.precioUnitario ?? curr.PrecioUnitario);
            return acc + (cant * precio);
        }, 0);

        const porcentaje = parseFloat(inputDescuento?.value) || 0;
        const descuento = (subtotal * porcentaje) / 100;
        const baseGravable = subtotal - descuento;
        const impuesto = baseGravable * 0.13;
        const total = baseGravable + impuesto;

        document.getElementById("lblSubtotal").innerText = "₡" + subtotal.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        document.getElementById("lblDescuento").innerText = "- ₡" + descuento.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        document.getElementById("lblBaseGravable").innerText = "₡" + baseGravable.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        document.getElementById("lblImpuesto").innerText = "₡" + impuesto.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        document.getElementById("lblTotal").innerText = "₡" + total.toLocaleString('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function mostrarErrorDetalle(msg) {
        if (!errorDetalle) return;
        if (msg) {
            errorDetalle.innerText = msg;
            errorDetalle.style.display = "block";
        } else {
            errorDetalle.style.display = "none";
        }
    }

    // Validación antes del envío
    if (formFactura) {
        formFactura.addEventListener("submit", function (e) {
            if (detalles.length === 0) {
                e.preventDefault();
                mostrarErrorDetalle("La factura debe contener al menos un producto en el detalle.");
                return false;
            }
        });
    }

    // muestra tabla al entrar a la vista
    renderizarTabla();
});