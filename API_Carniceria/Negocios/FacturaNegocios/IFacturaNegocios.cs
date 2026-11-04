using API_Carniceria.Entidades;

namespace API_Carniceria.Negocios.FacturaNegocios
{
    public interface IFacturaNegocios
    {
        Task<List<Factura>> ObtenerTodasAsync();
        Task<Factura?> ObtenerPorIdAsync(Guid id);
        Task<Factura?> ObtenerPorNumeroAsync(int numeroFactura);
        Task<Factura> CrearAsync(Factura factura);
    }//fn class
}//fn space
