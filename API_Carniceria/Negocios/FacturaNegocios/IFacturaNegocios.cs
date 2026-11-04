using API_Carniceria.Entidades;

namespace API_Carniceria.Negocios.FacturaNegocios
{
    public interface IFacturaNegocios
    {
        Task<List<Factura>> ObtenerTodasAsync();
        Task<Factura?> ObtenerPorIdAsync(Guid id);
        Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura);
        Task<List<Factura>> BuscarAsync(string criterio);
        Task<Factura> CrearAsync(Factura factura);
        Task<Factura> ActualizarAsync(Factura factura);
        Task<bool> EliminarAsync(Guid id);
        Task<Factura> PagarAsync(Guid id);
        Task<Factura> AnularAsync(Guid id);

    }//fn class
}//fn space
