using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Cliente_Repository;
using API_Carniceria.Repositories.Empleado_Repository;
using API_Carniceria.Repositories.Factura_Repository;
using API_Carniceria.Repositories.Producto_Repository;

namespace API_Carniceria.Negocios.FacturaNegocios
{
    public class FacturaNegocios : IFacturaNegocios
    {
        #region variablesinternas
        private readonly IFacturaRepository _facturaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IProductoRepository _productoRepository;
        private const decimal DESCUENTO_MAXIMO = 25m;
        #endregion

        public FacturaNegocios( IFacturaRepository facturaRepository,IClienteRepository clienteRepository,
            IEmpleadoRepository empleadoRepository,IProductoRepository productoRepository)
        {
            _facturaRepository = facturaRepository;
            _clienteRepository = clienteRepository;
            _empleadoRepository = empleadoRepository;
            _productoRepository = productoRepository;
        }

        public async Task<List<Factura>> ObtenerTodasAsync()
        {
            return await _facturaRepository.ObtenerTodasAsync();
        }

        public async Task<Factura?> ObtenerPorIdAsync(Guid id)
        {
            return await _facturaRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura)
        {
            return await _facturaRepository.ObtenerPorNumeroAsync(numeroFactura);
        }

        public async Task<List<Factura>> BuscarAsync(string criterio)
        {
            return await _facturaRepository.BuscarAsync(criterio);
        }

        public async Task<Factura> CrearAsync(Factura factura)
        {
            if (factura == null)
            {
                throw new ArgumentNullException(nameof(factura));
            }

            if (factura.PorcentajeDescuento < 0 || factura.PorcentajeDescuento > DESCUENTO_MAXIMO)
            {
                throw new Exception($"El porcentaje de descuento debe estar entre 0% y {DESCUENTO_MAXIMO}%.");
            }

            var cliente = await _clienteRepository.ObtenerPorIdAsync(factura.ClienteId);
            if (cliente == null)
            {
                throw new Exception("La persona cliente seleccionada no existe.");
            }

            var empleado = await _empleadoRepository.ObtenerPorIdAsync(factura.EmpleadoId);
            if (empleado == null)
            {
                throw new Exception("La persona empleada seleccionada no existe.");
            }

            if (factura.Detalles == null || !factura.Detalles.Any())
            {
                throw new Exception("La factura debe contener al menos un producto.");
            }

            factura.IdentificacionCliente = cliente.Identificacion;
            factura.NombreCliente = $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim();
            factura.NombreEmpleado = $"{empleado.Nombre} {empleado.PrimerApellido} {empleado.SegundoApellido}".Trim();
            factura.NumeroFactura = await _facturaRepository.ObtenerSiguienteNumeroFacturaAsync();
            factura.Fecha = DateTime.Now;
            factura.Estado = EstadoFactura.Pendiente;

            foreach (var detalle in factura.Detalles)
            {
                if (detalle.Cantidad <= 0)
                {
                    throw new Exception("La cantidad de cada producto debe ser mayor a cero.");
                }

                var producto = await _productoRepository.ObtenerPorIdAsync(detalle.ProductoId);
                if (producto == null)
                {
                    throw new Exception("Uno de los productos especificados en el detalle no existe.");
                }

                if (producto.ExistenciaActual < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto {producto.NombreProducto}. Stock disponible: {producto.ExistenciaActual}.");
                }

                detalle.FacturaId = factura.Id;
                detalle.CodigoProducto = producto.CodigoProducto;
                detalle.NombreProducto = producto.NombreProducto;
                detalle.UnidadMedida = producto.UnidadMedida;
                detalle.PrecioUnitario = producto.PrecioPorUnidadMedida;
            }

            await _facturaRepository.RegistrarAsync(factura);
            return factura;
        }

        public async Task<Factura> ActualizarAsync(Factura factura)
        {
            if (factura == null)
            {
                throw new ArgumentNullException(nameof(factura));
            }

            var actual = await _facturaRepository.ObtenerPorIdAsync(factura.Id);
            if (actual == null)
            {
                throw new Exception("La factura que desea actualizar no existe.");
            }

            if (actual.Estado == EstadoFactura.Anulada)
            {
                throw new Exception("Una factura en estado Anulada no puede ser modificada.");
            }

            if (factura.PorcentajeDescuento < 0 || factura.PorcentajeDescuento > DESCUENTO_MAXIMO)
            {
                throw new Exception($"El porcentaje de descuento debe estar entre 0% y {DESCUENTO_MAXIMO}%.");
            }

            var cliente = await _clienteRepository.ObtenerPorIdAsync(factura.ClienteId);
            if (cliente == null)
            {
                throw new Exception("La persona cliente seleccionada no existe.");
            }

            var empleado = await _empleadoRepository.ObtenerPorIdAsync(factura.EmpleadoId);
            if (empleado == null)
            {
                throw new Exception("La persona empleada seleccionada no existe.");
            }

            if (factura.Detalles == null || !factura.Detalles.Any())
            {
                throw new Exception("La factura debe contener al menos un producto.");
            }

            if (actual.Estado == EstadoFactura.Pagada)
            {
                foreach (var detalleAnterior in actual.Detalles)
                {
                    var prod = await _productoRepository.ObtenerPorIdAsync(detalleAnterior.ProductoId);
                    if (prod != null)
                    {
                        prod.ExistenciaActual += detalleAnterior.Cantidad;
                        await _productoRepository.ActualizarAsync(prod);
                    }
                }

                foreach (var detalleNuevo in factura.Detalles)
                {
                    var prod = await _productoRepository.ObtenerPorIdAsync(detalleNuevo.ProductoId);
                    if (prod == null)
                    {
                        throw new Exception("Uno de los productos especificados en el nuevo detalle no existe.");
                    }

                    if (prod.ExistenciaActual < detalleNuevo.Cantidad)
                    {
                        throw new Exception($"Stock insuficiente para el producto {prod.NombreProducto} tras la modificación. Stock disponible: {prod.ExistenciaActual}.");
                    }

                    prod.ExistenciaActual -= detalleNuevo.Cantidad;
                    await _productoRepository.ActualizarAsync(prod);
                }
            }
            else
            {
                foreach (var detalleNuevo in factura.Detalles)
                {
                    var prod = await _productoRepository.ObtenerPorIdAsync(detalleNuevo.ProductoId);
                    if (prod == null)
                    {
                        throw new Exception("Uno de los productos especificados no existe.");
                    }

                    if (prod.ExistenciaActual < detalleNuevo.Cantidad)
                    {
                        throw new Exception($"Stock insuficiente para el producto {prod.NombreProducto}. Stock disponible: {prod.ExistenciaActual}.");
                    }
                }
            }

            actual.ClienteId = factura.ClienteId;
            actual.IdentificacionCliente = cliente.Identificacion;
            actual.NombreCliente = $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim();
            actual.EmpleadoId = factura.EmpleadoId;
            actual.NombreEmpleado = $"{empleado.Nombre} {empleado.PrimerApellido} {empleado.SegundoApellido}".Trim();
            actual.MetodoPago = factura.MetodoPago;
            actual.PorcentajeDescuento = factura.PorcentajeDescuento;

            foreach (var det in factura.Detalles)
            {
                var prod = await _productoRepository.ObtenerPorIdAsync(det.ProductoId);
                det.FacturaId = actual.Id;
                det.CodigoProducto = prod!.CodigoProducto;
                det.NombreProducto = prod.NombreProducto;
                det.UnidadMedida = prod.UnidadMedida;
                det.PrecioUnitario = prod.PrecioPorUnidadMedida;
            }

            actual.Detalles = factura.Detalles;
            await _facturaRepository.ActualizarAsync(actual);
            return actual;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return false;
            }

            if (factura.Estado != EstadoFactura.Pendiente)
            {
                throw new Exception("Únicamente las facturas en estado Pendiente pueden ser eliminadas físicamente.");
            }

            return await _facturaRepository.EliminarAsync(id);
        }

        public async Task<Factura> PagarAsync(Guid id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                throw new Exception("La factura no existe.");
            }

            if (factura.Estado == EstadoFactura.Pagada)
            {
                throw new Exception("La factura ya se encuentra pagada.");
            }

            if (factura.Estado == EstadoFactura.Anulada)
            {
                throw new Exception("No se puede pagar una factura que ha sido anulada.");
            }

            foreach (var detalle in factura.Detalles)
            {
                var prod = await _productoRepository.ObtenerPorIdAsync(detalle.ProductoId);
                if (prod == null)
                {
                    throw new Exception($"El producto {detalle.NombreProducto} no existe.");
                }

                if (prod.ExistenciaActual < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para procesar el pago del producto {prod.NombreProducto}. Stock actual: {prod.ExistenciaActual}.");
                }
            }

            foreach (var detalle in factura.Detalles)
            {
                var prod = await _productoRepository.ObtenerPorIdAsync(detalle.ProductoId);
                if (prod != null)
                {
                    prod.ExistenciaActual -= detalle.Cantidad;
                    await _productoRepository.ActualizarAsync(prod);
                }
            }

            factura.Estado = EstadoFactura.Pagada;
            await _facturaRepository.ActualizarAsync(factura);
            return factura;
        }

        public async Task<Factura> AnularAsync(Guid id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                throw new Exception("La factura no existe.");
            }

            if (factura.Estado == EstadoFactura.Anulada)
            {
                throw new Exception("La factura ya se encuentra anulada.");
            }

            if (factura.Estado == EstadoFactura.Pagada)
            {
                foreach (var detalle in factura.Detalles)
                {
                    var prod = await _productoRepository.ObtenerPorIdAsync(detalle.ProductoId);
                    if (prod != null)
                    {
                        prod.ExistenciaActual += detalle.Cantidad;
                        await _productoRepository.ActualizarAsync(prod);
                    }
                }
            }

            factura.Estado = EstadoFactura.Anulada;
            await _facturaRepository.ActualizarAsync(factura);
            return factura;
        }
    
    }//fn class
}//fn space