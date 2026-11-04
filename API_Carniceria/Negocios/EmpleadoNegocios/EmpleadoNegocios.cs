using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Empleado_Repository;
using API_Carniceria.Repositories.Factura_Repository;
using System.Text.RegularExpressions;

namespace API_Carniceria.Negocios.EmpleadoNegocios
{
    public class EmpleadoNegocios : IEmpleadoNegocios
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IFacturaRepository _facturaRepository;

        public EmpleadoNegocios(IEmpleadoRepository empleadoRepository, IFacturaRepository facturaRepository)
        {
            _empleadoRepository = empleadoRepository;
            _facturaRepository = facturaRepository;
        } // fn

        public async Task<List<Empleado>> ObtenerTodosAsync()
        {
            return await _empleadoRepository.ObtenerTodosAsync();
        } // fn

        public async Task<Empleado?> ObtenerPorIdAsync(Guid id)
        {
            return await _empleadoRepository.ObtenerPorIdAsync(id);
        } // fn

        public async Task<Empleado?> BuscarPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return null;
            }

            return await _empleadoRepository.ObtenerPorIdentificacionAsync(identificacion.Trim());
        } // fn

        public async Task<Empleado> CrearAsync(Empleado empleado)
        {
            await ValidarEmpleadoAsync(empleado);
            await _empleadoRepository.RegistrarAsync(empleado);
            return empleado;
        } // fn

        public async Task<bool> ActualizarAsync(Empleado empleado)
        {
            Empleado? actual = await _empleadoRepository.ObtenerPorIdAsync(empleado.Id);

            if (actual == null)
            {
                return false;
            }

            await ValidarEmpleadoAsync(empleado);
            await _empleadoRepository.ActualizarAsync(empleado);
            return true;
        } // fn

        public async Task<bool> EliminarAsync(Guid id)
        {
            Empleado? actual = await _empleadoRepository.ObtenerPorIdAsync(id);

            if (actual == null)
            {
                return false;
            }

            var tieneFacturas = await _facturaRepository.EmpleadoTieneFacturasAsync(id);
            if (tieneFacturas)
            {
                throw new Exception("No se puede eliminar la persona empleada porque tiene facturas asociadas.");
            }

            await _empleadoRepository.EliminarAsync(id);
            return true;
        } // fn

        #region MetodosInternos

        private async Task ValidarEmpleadoAsync(Empleado empleado)
        {
            // 1. Validamos campos de texto 
            if (string.IsNullOrWhiteSpace(empleado.Nombre) || empleado.Nombre.Trim().Length < 3 || empleado.Nombre.Trim().Length > 50)
            {
                throw new Exception("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(empleado.PrimerApellido) || empleado.PrimerApellido.Trim().Length < 3 || empleado.PrimerApellido.Trim().Length > 75)
            {
                throw new Exception("El primer apellido es obligatorio y debe tener entre 3 y 75 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(empleado.SegundoApellido) || empleado.SegundoApellido.Trim().Length < 3 || empleado.SegundoApellido.Trim().Length > 75)
            {
                throw new Exception("El segundo apellido es obligatorio y debe tener entre 3 y 75 caracteres.");
            }

            // 2. Validamos formato de identificación según su tipo
            if (string.IsNullOrWhiteSpace(empleado.Identificacion))
            {
                throw new Exception("El número de identificación es obligatorio.");
            }

            bool formatoValido;

            switch (empleado.TipoIdentificacion)
            {
                case TipoIdentificacion.CedulaNacional:
                    formatoValido = Regex.IsMatch(empleado.Identificacion, @"^\d-\d{4}-\d{4}$");
                    break;
                case TipoIdentificacion.Dimex:
                    formatoValido = Regex.IsMatch(empleado.Identificacion, @"^\d{12}$");
                    break;
                case TipoIdentificacion.Pasaporte:
                    formatoValido = Regex.IsMatch(empleado.Identificacion, @"^[a-zA-Z0-9]{6,15}$");
                    break;
                default:
                    formatoValido = false;
                    break;
            }

            if (!formatoValido)
            {
                throw new Exception("El formato de la identificación no es válido para el tipo seleccionado.");
            }

            // 3. Validar identificación única
            bool yaExiste = await _empleadoRepository.YaExisteEstaIdentificacionAsync(empleado.Identificacion, empleado.Id);
            if (yaExiste)
            {
                throw new Exception("Ya existe una persona empleada registrada con esta identificación.");
            }

            // 4. Validar fecha de nacimiento
            if (empleado.FechaNacimiento > DateTime.Today)
            {
                throw new Exception("La fecha de nacimiento no puede ser una fecha futura.");
            }

            if (empleado.FechaNacimiento > DateTime.Today.AddYears(-18))
            {
                throw new Exception("La persona empleada debe ser mayor de edad (al menos 18 años).");
            }

            // 5. Validar salario mensual
            if (empleado.SalarioMensual <= 0)
            {
                throw new Exception("El salario mensual debe ser un valor mayor que cero.");
            }

            // 6. Validar fecha de ingreso
            if (empleado.FechaIngreso > DateTime.Today)
            {
                throw new Exception("La fecha de ingreso no puede ser una fecha futura.");
            }

            // 7. Validar relación lógica entre Área de Trabajo y Puesto (Requisito de rúbrica)
            ValidarRelacionAreaPuesto(empleado.AreaTrabajo, empleado.Puesto);
        } // fn

        private void ValidarRelacionAreaPuesto(AreaTrabajo area, Puesto puesto)
        {
            if (!PuestoPerteneceAArea(area, puesto))
            {
                throw new Exception("El puesto seleccionado no corresponde al área de trabajo asignada.");
            }
        } // fn

        private static bool PuestoPerteneceAArea(AreaTrabajo area, Puesto puesto)
        {
            return area switch
            {
                AreaTrabajo.Administracion => puesto is Puesto.Administrador or Puesto.AsistenteAdministrativo,
                AreaTrabajo.VentasYCaja => puesto is Puesto.Cajero or Puesto.Vendedor or Puesto.ServicioAlCliente,
                AreaTrabajo.CorteYPreparacion => puesto is Puesto.Carnicero or Puesto.Despostador or Puesto.Empacador,
                AreaTrabajo.BodegaEInventario => puesto is Puesto.Bodeguero or Puesto.AuxiliarDeInventario,
                AreaTrabajo.Distribucion => puesto is Puesto.Conductor or Puesto.AyudanteDeDistribucion,
                AreaTrabajo.Limpieza => puesto is Puesto.PersonalDeLimpieza,
                _ => false
            };
        } // fn

        #endregion
    
    } // fn class
}
