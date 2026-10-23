using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Proveedor_Repository
{
    public interface IProveedorRepository
    {
        Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync();

    }//fin interface

}// fin space
