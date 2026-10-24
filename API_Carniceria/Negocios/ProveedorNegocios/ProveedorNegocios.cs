using API_Carniceria.Entidades;
using API_Carniceria.Repositories.Proveedor_Repository;

namespace API_Carniceria.Services.Proveedor_Service
{
    public class ProveedorNegocios : IProveedorNegocios
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedorNegocios(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync()
        {
            return _proveedorRepository.ObtenerTodosAsync();
        }


    }// fn class
}// fn space
