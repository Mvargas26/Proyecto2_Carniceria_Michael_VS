using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Factura_Repository
{
    public interface IFacturaRepository
    {
        Task<List<Factura>> ObtenerTodasAsync();
        Task<Factura?> ObtenerPorIdAsync(Guid id);
        Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura);
        Task<List<Factura>> BuscarAsync(string criterio);
        Task RegistrarAsync(Factura factura);
        Task ActualizarAsync(Factura factura);
        Task<bool> EliminarAsync(Guid id);
        Task<string> ObtenerSiguienteNumeroFacturaAsync();
        Task<bool> ClienteTieneFacturasAsync(Guid clienteId);
        Task<bool> EmpleadoTieneFacturasAsync(Guid empleadoId);
        Task<bool> ProductoTieneFacturasAsync(Guid productoId);

    }//fn class
}//fn space
