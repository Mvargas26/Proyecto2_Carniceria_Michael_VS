using API_Carniceria.Entidades;

namespace API_Carniceria.Negocios.ClienteNegocios
{
    public interface IClienteNegocios
    {
        Task<List<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(Guid id);
        Task<Cliente?> BuscarPorIdentificacionAsync(string identificacion);
        Task<Cliente> CrearAsync(Cliente cliente);
        Task<bool> ActualizarAsync(Cliente cliente);
        Task<bool> EliminarAsync(Guid id);
    } // fn interface
}
