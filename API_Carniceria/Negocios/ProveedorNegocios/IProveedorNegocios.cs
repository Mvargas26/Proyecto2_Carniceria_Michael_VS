using API_Carniceria.Entidades;

namespace API_Carniceria.Services.Proveedor_Service
{
    public interface IProveedorNegocios
    {
        Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync();
        Task<Proveedor?> ObtenerPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Proveedor>> BuscarAsync(string texto, string criterio);
        Task<Proveedor> CrearAsync(Proveedor proveedor);
        Task<bool> ActualizarAsync(Proveedor proveedor);

    }//fin interface
}//fn space
