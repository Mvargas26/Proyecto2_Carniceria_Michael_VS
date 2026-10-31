using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Cliente_Repository
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(Guid id);
        Task<Cliente?> ObtenerPorIdentificacionAsync(string identificacion);
        Task RegistrarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(Guid id);
        Task<bool> YaExisteEstaIdentificacionAsync(string identificacion, Guid? excluirId = null);
    }//fn interface
}//fn space
