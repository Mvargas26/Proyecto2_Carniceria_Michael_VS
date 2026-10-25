using API_Carniceria.Entidades;
using API_Carniceria.Enums;
using API_Carniceria.Repositories.Proveedor_Repository;
using System.Text.RegularExpressions;

namespace API_Carniceria.Services.Proveedor_Service
{
    public class ProveedorNegocios : IProveedorNegocios
    {
        //variables internas
        private readonly IProveedorRepository _proveedorRepository;

        //contru
        public ProveedorNegocios(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }//fn

        public Task<IReadOnlyCollection<Proveedor>> ObtenerTodosAsync()
        {
            return _proveedorRepository.ObtenerTodosAsync();
        }//fn

        public Task<Proveedor?> ObtenerPorIdAsync(Guid id)
        {
            return _proveedorRepository.ObtenerPorIdAsync(id);
        }//fn
        public Task<IReadOnlyCollection<Proveedor>> BuscarAsync(string texto, string criterio)
        {
            return _proveedorRepository.BuscarAsync(texto, criterio);
        }//fn

        public async Task<Proveedor> CrearAsync(Proveedor proveedor)
        {
            await ValidarProveedorAsync(proveedor);
            return await _proveedorRepository.CrearAsync(proveedor);
        }//fn

        public async Task<bool> ActualizarAsync(Proveedor proveedor)
        {
            Proveedor? actual = await _proveedorRepository.ObtenerPorIdAsync(proveedor.Id);

            if (actual == null)
            {
                return false;
            }

            await ValidarProveedorAsync(proveedor);
            return await _proveedorRepository.ActualizarAsync(proveedor);
        }//fn

        #region MetodosInternos

        // Revisa las reglas del P1; lanza un error con el motivo si alguna falla.
        private async Task ValidarProveedorAsync(Proveedor proveedor)
        {
            bool formatoValido;

            switch (proveedor.TipoIdentificacion)
            {
                case TipoIdentificacion.CedulaNacional:
                    formatoValido = Regex.IsMatch(proveedor.Identificacion, @"^\d-\d{4}-\d{4}$");
                    break;
                case TipoIdentificacion.CedulaJuridica:
                    formatoValido = Regex.IsMatch(proveedor.Identificacion, @"^3-\d{3}-\d{6}$");
                    break;
                case TipoIdentificacion.Dimex:
                    formatoValido = Regex.IsMatch(proveedor.Identificacion, @"^\d{12}$");
                    break;
                default:
                    formatoValido = false;
                    break;
            }

            if (!formatoValido)
            {
                throw new Exception("El formato de la identificación no es válido para el tipo seleccionado.");
            }

            var proveedores = await _proveedorRepository.ObtenerTodosAsync();

            bool identificacionRepetida = false;

            foreach (Proveedor existente in proveedores)
            {
                bool mismaIdentificacion = existente.Identificacion == proveedor.Identificacion;
                bool esOtroProveedor = existente.Id != proveedor.Id;

                if (mismaIdentificacion && esOtroProveedor)
                {
                    identificacionRepetida = true;
                    break;
                }
            }

            if (identificacionRepetida)
            {
                throw new Exception("Ya existe un proveedor registrado con esta identificación.");
            }

            if (!Regex.IsMatch(proveedor.Telefono, @"^\d{8}$"))
            {
                throw new Exception("El teléfono debe contener exactamente 8 dígitos numéricos.");
            }
        }
           
        #endregion

    }// fn class
}// fn space
