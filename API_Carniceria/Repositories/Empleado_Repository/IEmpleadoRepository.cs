using API_Carniceria.Entidades;

namespace API_Carniceria.Repositories.Empleado_Repository
{
    public interface IEmpleadoRepository
    {
        Task<List<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(Guid id);
        Task<Empleado?> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> YaExisteEstaIdentificacionAsync(string identificacion, Guid idExcluir);
        Task RegistrarAsync(Empleado empleado);
        Task ActualizarAsync(Empleado empleado);
        Task EliminarAsync(Guid id);

    }//fn class
}
