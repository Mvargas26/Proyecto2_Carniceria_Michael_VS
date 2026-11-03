using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Cliente_Repository;
using System.Text.RegularExpressions;

namespace API_Carniceria.Negocios.ClienteNegocios
{
    public class ClienteNegocios : IClienteNegocios
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteNegocios(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        } // fn

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepository.ObtenerTodosAsync();
        } // fn

        public async Task<Cliente?> ObtenerPorIdAsync(Guid id)
        {
            return await _clienteRepository.ObtenerPorIdAsync(id);
        } // fn

        public async Task<Cliente?> BuscarPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return null;
            }

            return await _clienteRepository.ObtenerPorIdentificacionAsync(identificacion.Trim());
        } // fn

        public async Task<Cliente> CrearAsync(Cliente cliente)
        {
            await ValidarClienteAsync(cliente);
            await _clienteRepository.RegistrarAsync(cliente);
            return cliente;
        } // fn

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            Cliente? actual = await _clienteRepository.ObtenerPorIdAsync(cliente.Id);

            if (actual == null)
            {
                return false;
            }

            await ValidarClienteAsync(cliente);
            await _clienteRepository.ActualizarAsync(cliente);
            return true;
        } // fn

        public async Task<bool> EliminarAsync(Guid id)
        {
            Cliente? actual = await _clienteRepository.ObtenerPorIdAsync(id);

            if (actual == null)
            {
                return false;
            }

            // falta Validar que no tenga facturas asociadas como con el prove
            // if (tieneFacturasAsociadas) { throw new Exception("No es posible eliminar el cliente porque tiene facturas asociadas."); }

            await _clienteRepository.EliminarAsync(id);
            return true;
        } // fn

        #region MetodosInternos

        private async Task ValidarClienteAsync(Cliente cliente)
        {
            // 1. Validar campos de texto y longitudes
            if (string.IsNullOrWhiteSpace(cliente.Nombre) || cliente.Nombre.Trim().Length < 3 || cliente.Nombre.Trim().Length > 50)
            {
                throw new Exception("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(cliente.PrimerApellido) || cliente.PrimerApellido.Trim().Length < 3 || cliente.PrimerApellido.Trim().Length > 75)
            {
                throw new Exception("El primer apellido es obligatorio y debe tener entre 3 y 75 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(cliente.SegundoApellido) || cliente.SegundoApellido.Trim().Length < 3 || cliente.SegundoApellido.Trim().Length > 75)
            {
                throw new Exception("El segundo apellido es obligatorio y debe tener entre 3 y 75 caracteres.");
            }

            // 2. Validar formato de identificación según su tipo
            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
            {
                throw new Exception("El número de identificación es obligatorio.");
            }

            bool formatoValido;

            switch (cliente.TipoIdentificacion)
            {
                case TipoIdentificacion.CedulaNacional:
                    formatoValido = Regex.IsMatch(cliente.Identificacion, @"^\d-\d{4}-\d{4}$");
                    break;
                case TipoIdentificacion.Dimex:
                    formatoValido = Regex.IsMatch(cliente.Identificacion, @"^\d{12}$");
                    break;
                case TipoIdentificacion.CedulaJuridica:
                    formatoValido = Regex.IsMatch(cliente.Identificacion, @"^3-\d{3}-\d{6}$");
                    break;
                case TipoIdentificacion.Pasaporte:
                    formatoValido = Regex.IsMatch(cliente.Identificacion, @"^[a-zA-Z0-9]{6,15}$");
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
            bool yaExiste = await _clienteRepository.YaExisteEstaIdentificacionAsync(cliente.Identificacion, cliente.Id);
            if (yaExiste)
            {
                throw new Exception("Ya existe una persona cliente registrada con esta identificación.");
            }

            // 4. Validar fecha de nacimiento
            if (cliente.FechaNacimiento > DateTime.Today)
            {
                throw new Exception("La fecha de nacimiento no puede ser una fecha futura.");
            }
        } // fn

        #endregion
    } // fn class
}
