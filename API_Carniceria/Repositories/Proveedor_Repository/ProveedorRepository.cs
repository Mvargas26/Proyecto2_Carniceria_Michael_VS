using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Proveedor_Repository
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly List<Proveedor> _proveedores = new();

        public Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync()
        {
            IReadOnlyCollection<Proveedor> resultado = _proveedores.ToList();
            return Task.FromResult(resultado);
        }


    }//fn class
}//fn space
