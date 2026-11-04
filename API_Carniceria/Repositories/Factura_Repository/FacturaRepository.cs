using API_Carniceria.Entidades;
using API_Carniceria.Enums;

namespace API_Carniceria.Repositories.Factura_Repository
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly List<Factura> _facturas = new();

        public FacturaRepository()
        {
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

            var facturaId = Guid.NewGuid();
            var facturaEjemplo = new Factura
            {
                Id = facturaId,
                NumeroFactura = "FAC-0001",
                Fecha = DateTime.Now,
                ClienteId = Guid.NewGuid(),
                IdentificacionCliente = "1-1111-1111",
                NombreCliente = "Carlos Mora Solís",
                EmpleadoId = Guid.NewGuid(),
                NombreEmpleado = "Ana Gómez Rojas",
                MetodoPago = MetodoPago.Efectivo,
                Estado = EstadoFactura.Pendiente,
                PorcentajeDescuento = 0,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = facturaId,
                        ProductoId = Guid.NewGuid(),
                        CodigoProducto = "PROD-001",
                        NombreProducto = "Bistec de Res",
                        UnidadMedida = UnidadMedida.Kilogramo,
                        Cantidad = 2,
                        PrecioUnitario = 4500
                    },
                    new DetalleFactura
                    {
                        Id = Guid.NewGuid(),
                        FacturaId = facturaId,
                        ProductoId = Guid.NewGuid(),
                        CodigoProducto = "PROD-002",
                        NombreProducto = "Chuleta de Cerdo",
                        UnidadMedida = UnidadMedida.Kilogramo,
                        Cantidad = 1.5m,
                        PrecioUnitario = 3800
                    }
                }
            };

            _facturas.Add(facturaEjemplo);
        }
        #endregion
    }
}
