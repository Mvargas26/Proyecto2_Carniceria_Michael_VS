using API_Carniceria.Entidades;
using API_Carniceria.Enums;

namespace API_Carniceria.Repositories.Empleado_Repository
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly List<Empleado> _empleados = new();

        //constru
        public EmpleadoRepository()
        {
            AgregarEmpleadoEjemplo();
        } // fn


        public Task<List<Empleado>> ObtenerTodosAsync()
        {
            return Task.FromResult(_empleados.ToList());
        } // fn

        public Task<Empleado?> ObtenerPorIdAsync(Guid id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(empleado);
        } // fn

        public Task<Empleado?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            var empleado = _empleados.FirstOrDefault(e =>
                e.Identificacion.Trim().Equals(identificacion.Trim(), StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(empleado);
        } // fn

        public Task<bool> YaExisteEstaIdentificacionAsync(string identificacion, Guid idExcluir)
        {
            bool existe = _empleados.Any(e =>
                e.Identificacion.Trim().Equals(identificacion.Trim(), StringComparison.OrdinalIgnoreCase) &&
                e.Id != idExcluir);

            return Task.FromResult(existe);
        } // fn

        public Task RegistrarAsync(Empleado empleado)
        {
            if (empleado.Id == Guid.Empty)
            {
                empleado.Id = Guid.NewGuid();
            }

            _empleados.Add(empleado);
            return Task.CompletedTask;
        } // fn

        public Task ActualizarAsync(Empleado empleado)
        {
            var indice = _empleados.FindIndex(e => e.Id == empleado.Id);

            if (indice != -1)
            {
                _empleados[indice] = empleado;
            }

            return Task.CompletedTask;
        } // fn

        public Task EliminarAsync(Guid id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);

            if (empleado != null)
            {
                _empleados.Remove(empleado);
            }

            return Task.CompletedTask;
        } // fn

        #region metodos internos
        private void AgregarEmpleadoEjemplo()
        {
            var empleadoEjemplo = new Empleado
            {
                Id = Guid.NewGuid(),
                TipoIdentificacion = TipoIdentificacion.CedulaNacional,
                Identificacion = "1-1234-0567",
                Nombre = "Ana",
                PrimerApellido = "Gómez",
                SegundoApellido = "Rojas",
                FechaNacimiento = new DateTime(1990, 5, 15),
                SalarioMensual = 450000m,
                FechaIngreso = DateTime.Today.AddYears(-2),
                AreaTrabajo = AreaTrabajo.CorteYPreparacion, 
                Puesto = Puesto.Carnicero,           
                Turno = Turno.Mixto              
            };

            _empleados.Add(empleadoEjemplo);
        } // fn

        #endregion

    } // fn class


}//fn space
