using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Cliente_Repository;
using API_Carniceria.Repositories.Empleado_Repository;
using API_Carniceria.Repositories.Producto_Repository;

namespace API_Carniceria.Repositories.Factura_Repository
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly List<Factura> _facturas = new();
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IProductoRepository _productoRepository;

        public FacturaRepository(IClienteRepository clienteRepository, IEmpleadoRepository empleadoRepository, IProductoRepository productoRepository)
        {
            _clienteRepository = clienteRepository;
            _empleadoRepository = empleadoRepository;
            _productoRepository = productoRepository;
            AgregarFacturaEjemplo();
        }

        public Task<List<Factura>> ObtenerTodasAsync()
        {
            return Task.FromResult(_facturas.ToList());
        }

        public Task<Factura?> ObtenerPorIdAsync(Guid id)
        {
            var factura = _facturas.FirstOrDefault(f => f.Id == id);
            return Task.FromResult(factura);
        }

        public Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura)
        {
            var factura = _facturas.FirstOrDefault(f => f.NumeroFactura.Equals(numeroFactura, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(factura);
        }

        public Task<List<Factura>> BuscarAsync(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                return Task.FromResult(new List<Factura>());
            }

            criterio = criterio.Trim();
            var resultados = _facturas
                .Where(f => f.NumeroFactura.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                            f.IdentificacionCliente.Contains(criterio, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Task.FromResult(resultados);
        }

        public Task RegistrarAsync(Factura factura)
        {
            _facturas.Add(factura);
            return Task.CompletedTask;
        }

        public Task ActualizarAsync(Factura factura)
        {
            var indice = _facturas.FindIndex(f => f.Id == factura.Id);
            if (indice != -1)
            {
                _facturas[indice] = factura;
            }
            return Task.CompletedTask;
        }

        public Task<bool> EliminarAsync(Guid id)
        {
            var factura = _facturas.FirstOrDefault(f => f.Id == id);
            if (factura == null)
            {
                return Task.FromResult(false);
            }

            _facturas.Remove(factura);
            return Task.FromResult(true);
        }

        public Task<string> ObtenerSiguienteNumeroFacturaAsync()
        {
            int siguiente = _facturas.Count + 1;
            string codigo = $"FAC-{siguiente:D4}";
            while (_facturas.Any(f => f.NumeroFactura.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
            {
                siguiente++;
                codigo = $"FAC-{siguiente:D4}";
            }
            return Task.FromResult(codigo);
        }

        public Task<bool> ClienteTieneFacturasAsync(Guid clienteId)
        {
            bool tiene = _facturas.Any(f => f.ClienteId == clienteId);
            return Task.FromResult(tiene);
        }

        public Task<bool> EmpleadoTieneFacturasAsync(Guid empleadoId)
        {
            bool tiene = _facturas.Any(f => f.EmpleadoId == empleadoId);
            return Task.FromResult(tiene);
        }

        public Task<bool> ProductoTieneFacturasAsync(Guid productoId)
        {
            bool tiene = _facturas.Any(f => f.Detalles != null && f.Detalles.Any(d => d.ProductoId == productoId));
            return Task.FromResult(tiene);
        }

        #region Metodos Internos

        private void AgregarFacturaEjemplo()
        {
            if (_facturas.Any())
            {
                return;
            }

            // Traemos las listas asincronas en memoria
            var listaClientes = _clienteRepository.ObtenerTodosAsync().GetAwaiter().GetResult();
            var listaEmpleados = _empleadoRepository.ObtenerTodosAsync().GetAwaiter().GetResult();
            var listaProductos = _productoRepository.ObtenerTodosAsync().GetAwaiter().GetResult();

            // Extraemos los registros reales con LINQ
            var cliente = listaClientes.FirstOrDefault();
            var empleado = listaEmpleados.FirstOrDefault();
            var productos = listaProductos.Take(2).ToList();

            // Si los repositorios aún no tienen datos, no se puede enlazar
            if (cliente == null || empleado == null || productos.Count < 2)
            {
                return;
            }

            var prod1 = productos[0];
            var prod2 = productos[1];

            var facturaId = Guid.NewGuid();

            var facturaEjemplo = new Factura
            {
                Id = facturaId,
                NumeroFactura = "FAC-0001",
                Fecha = DateTime.Now,
                ClienteId = cliente.Id,
                IdentificacionCliente = cliente.Identificacion,
                NombreCliente = cliente.Nombre + " " + cliente.PrimerApellido,
                EmpleadoId = empleado.Id,
                NombreEmpleado = empleado.Nombre + " " + empleado.PrimerApellido,
                MetodoPago = MetodoPago.Efectivo,
                Estado = EstadoFactura.Pendiente,
                PorcentajeDescuento = 0,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = facturaId,
                        ProductoId = prod1.Id,
                        CodigoProducto = prod1.CodigoProducto,
                        NombreProducto = prod1.NombreProducto,
                        UnidadMedida = prod1.UnidadMedida,
                        Cantidad = 2,
                        PrecioUnitario = prod1.PrecioPorUnidadMedida
                    },
                    new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = facturaId,
                        ProductoId = prod2.Id,
                        CodigoProducto = prod2.CodigoProducto,
                        NombreProducto = prod2.NombreProducto,
                        UnidadMedida = prod2.UnidadMedida,
                        Cantidad = 1.5m,
                        PrecioUnitario = prod2.PrecioPorUnidadMedida
                    }
                }
            };

            _facturas.Add(facturaEjemplo);
        }//fn 

        #endregion
    }
}
