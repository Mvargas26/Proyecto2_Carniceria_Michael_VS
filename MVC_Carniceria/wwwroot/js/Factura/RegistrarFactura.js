document.addEventListener("DOMContentLoaded", function () {
    let detalles = [];

    const selectProducto = document.getElementById("selectProducto");
    const txtExistencia = document.getElementById("txtExistencia");
    const txtPrecio = document.getElementById("txtPrecio");
    const inputCantidad = document.getElementById("inputCantidad");
    const btnAgregarLinea = document.getElementById("btnAgregarLinea");
    const errorDetalle = document.getElementById("errorDetalle");
    const inputDescuento = document.getElementById("inputDescuento");
    const cuerpoTabla = document.getElementById("cuerpoTablaDetalles");
    const formFactura = document.getElementById("formFactura");

    // Autocompleta datos al seleccionar un producto
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

            const prod = productosCatalogo.find(p => (p.Id || p.id).toLowerCase() === prodId.toLowerCase());
            if (!prod) return;

            // Propiedades reales del modelo
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

            // Si ya existe la línea, acumulamos respetando el stock
            const detalleExistente = detalles.find(d => d.productoId.toLowerCase() === prodId.toLowerCase());
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

    // negocios: Descuento (Máx. 25%)
    if (inputDescuento) {
        inputDescuento.addEventListener("input", function () {
            let val = parseFloat(this.value);
            if (val < 0) this.value = 0;
            if (val > 25) this.value = 25; 
            calcularTotales();
        });
    }

    //Renderizar tabla y reasignar índices
    function renderizarTabla() {
        if (!cuerpoTabla) return;
        cuerpoTabla.innerHTML = "";

        if (detalles.length === 0) {
            cuerpoTabla.innerHTML = `<tr><td colspan="7" class="text-center text-muted">No se han agregado productos a la factura.</td></tr>`;
            calcularTotales();
            return;
        }

        detalles.forEach((det, i) => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>
                    <input type="hidden" name="Detalles[${i}].ProductoId" value="${det.productoId}" />
                    <input type="hidden" name="Detalles[${i}].CodigoProducto" value="${det.codigo}" />
                    <input type="hidden" name="Detalles[${i}].NombreProducto" value="${det.nombre}" />
                    <input type="hidden" name="Detalles[${i}].UnidadMedida" value="${det.unidadMedida}" />
                    <input type="hidden" name="Detalles[${i}].Cantidad" value="${det.cantidad}" />
                    <input type="hidden" name="Detalles[${i}].PrecioUnitario" value="${det.precioUnitario}" />
                    ${det.codigo}
                </td>
                <td>${det.nombre}</td>
                <td>${det.unidadMedida}</td>
                <td class="text-end">${det.cantidad.toFixed(2)}</td>
                <td class="text-end">₡${Number(det.precioUnitario).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                <td class="text-end">₡${Number(det.subtotal).toLocaleString('es-CR', { minimumFractionDigits: 2 })}</td>
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-icon btn-delete btn-quitar-detalle" data-indice="${i}" title="Quitar">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            `;
            cuerpoTabla.appendChild(tr);
        });

        // Eventos para botones de eliminar fila
        document.querySelectorAll(".btn-quitar-detalle").forEach(btn => {
            btn.addEventListener("click", function () {
                const idx = parseInt(this.getAttribute("data-indice"));
                detalles.splice(idx, 1);
                renderizarTabla();
            });
        });

        calcularTotales();
    }

    // Cálculos automáticos de totales
    function calcularTotales() {
        const subtotal = detalles.reduce((acc, curr) => acc + curr.subtotal, 0);
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

    //Validación previa al envío
    if (formFactura) {
        formFactura.addEventListener("submit", function (e) {
            if (detalles.length === 0) {
                e.preventDefault();
                mostrarErrorDetalle("La factura debe contener al menos un producto en el detalle.");
                return false;
            }
        });
    }

    // Inicializar tabla
    renderizarTabla();
});