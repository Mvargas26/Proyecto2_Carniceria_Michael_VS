using API_Carniceria.Entidades;
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
        
        #endregion

        //constru
        public FacturaNegocios( IFacturaRepository facturaRepository,IClienteRepository clienteRepository,
            IEmpleadoRepository empleadoRepository,IProductoRepository productoRepository)
        {
            _facturaRepository = facturaRepository;
            _clienteRepository = clienteRepository;
            _empleadoRepository = empleadoRepository;
            _productoRepository = productoRepository;
        }//fn constru

        public async Task<List<Factura>> ObtenerTodasAsync()
        {
            return await _facturaRepository.ObtenerTodasAsync();
        }

        public async Task<Factura?> ObtenerPorIdAsync(Guid id)
        {
            return await _facturaRepository.ObtenerPorIdAsync(id);
        }//fn

        public async Task<Factura?> ObtenerPorNumeroAsync(int numeroFactura)
        {
            return await _facturaRepository.ObtenerPorNumeroAsync(numeroFactura);
        }//fn

        public async Task<Factura> CrearAsync(Factura factura)
        {
            if (factura == null)
            {
                throw new ArgumentNullException(nameof(factura));
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

            factura.NombreCliente = $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim();
            factura.NombreEmpleado = $"{empleado.Nombre} {empleado.PrimerApellido} {empleado.SegundoApellido}".Trim();
            factura.NumeroFactura = await _facturaRepository.ObtenerSiguienteNumeroFacturaAsync();
            factura.Fecha = DateTime.Now;

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
                detalle.NombreProducto = producto.NombreProducto;
                if (detalle.PrecioUnitario <= 0)
                {
                    detalle.PrecioUnitario = producto.PrecioPorUnidadMedida;
                }

                producto.ExistenciaActual -= detalle.Cantidad;
                await _productoRepository.ActualizarAsync(producto);
            }

            await _facturaRepository.RegistrarAsync(factura);
            return factura;
        }
    
    }//fin clas
}//fn space
