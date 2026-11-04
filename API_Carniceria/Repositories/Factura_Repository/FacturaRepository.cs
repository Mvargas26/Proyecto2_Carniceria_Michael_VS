using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Factura_Repository
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly List<Factura> _facturas = new();

        //constru
        public FacturaRepository()
        {
            AgregarFacturaEjemplo();
        }//fn constru
        public Task<List<Factura>> ObtenerTodasAsync()
        {
            return Task.FromResult(_facturas.ToList());
        }//fn

        public Task<Factura?> ObtenerPorIdAsync(Guid id)
        {
            var factura = _facturas.FirstOrDefault(f => f.Id == id);
            return Task.FromResult(factura);
        }//fn

        public Task<Factura?> ObtenerPorNumeroAsync(int numeroFactura)
        {
            var factura = _facturas.FirstOrDefault(f => f.NumeroFactura == numeroFactura);
            return Task.FromResult(factura);
        }//fn

        public Task RegistrarAsync(Factura factura)
        {
            _facturas.Add(factura);
            return Task.CompletedTask;
        }//fn

        public Task<int> ObtenerSiguienteNumeroFacturaAsync()
        {
            int siguiente = _facturas.Any() ? _facturas.Max(f => f.NumeroFactura) + 1 : 1;
            return Task.FromResult(siguiente);
        }//fn

        public Task<bool> ClienteTieneFacturasAsync(Guid clienteId)
        {
            bool tiene = _facturas.Any(f => f.ClienteId == clienteId);
            return Task.FromResult(tiene);
        }//fn

        public Task<bool> EmpleadoTieneFacturasAsync(Guid empleadoId)
        {
            bool tiene = _facturas.Any(f => f.EmpleadoId == empleadoId);
            return Task.FromResult(tiene);
        }//fn

        public Task<bool> ProductoTieneFacturasAsync(Guid productoId)
        {
            bool tiene = _facturas.Any(f => f.Detalles != null && f.Detalles.Any(d => d.ProductoId == productoId));
            return Task.FromResult(tiene);
        }//fn

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
                NumeroFactura = 1,
                Fecha = DateTime.Now,
                ClienteId = Guid.NewGuid(),
                NombreCliente = "Carlos Mora Solís",
                EmpleadoId = Guid.NewGuid(),
                NombreEmpleado = "Ana Gómez Rojas",
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        FacturaId = facturaId,
                        ProductoId = Guid.NewGuid(),
                        NombreProducto = "Bistec de Res",
                        Cantidad = 2,
                        PrecioUnitario = 4500
                    }
                }
            };

            _facturas.Add(facturaEjemplo);
        }

        #endregion

    }
}
