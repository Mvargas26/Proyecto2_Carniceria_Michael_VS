using API_Carniceria.Entidades;

namespace API_Carniceria.Negocios.EmpleadoNegocios
{
    public interface IEmpleadoNegocios
    {
        Task<List<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(Guid id);
        Task<Empleado?> BuscarPorIdentificacionAsync(string identificacion);
        Task<Empleado> CrearAsync(Empleado empleado);
        Task<bool> ActualizarAsync(Empleado empleado);
        Task<bool> EliminarAsync(Guid id);
    
    }//fn class
}//fn space
