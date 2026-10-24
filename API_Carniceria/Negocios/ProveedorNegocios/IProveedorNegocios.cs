using API_Carniceria.Entidades;

namespace API_Carniceria.Services.Proveedor_Service
{
    public interface IProveedorNegocios
    {
        Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync();


    }//fin interface
}//fn space
