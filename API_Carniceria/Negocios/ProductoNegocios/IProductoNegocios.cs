using API_Carniceria.Entidades;

namespace API_Carniceria.Negocios.ProductoNegocios
{
    public interface IProductoNegocios
    {
        Task<IReadOnlyCollection<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Producto>> BuscarAsync(string texto, string criterio);
        Task<Producto> CrearAsync(Producto producto);
        Task<bool> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(Guid id);


    }//fn class
}//fn space
