using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Proveedor_Repository
{
    public interface IProveedorRepository
    {
        Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync();
        Task<Proveedor?> ObtenerPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Proveedor>> BuscarAsync(string texto, string criterio);

        Task<Proveedor> CrearAsync(Proveedor proveedor);

        Task<bool> ActualizarAsync(Proveedor proveedor);







    }//fin interface

}// fin space
