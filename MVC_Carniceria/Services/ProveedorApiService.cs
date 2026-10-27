using MVC_Carniceria.Models;

namespace MVC_Carniceria.Services
{
    public class ProveedorApiService 
    {
        private readonly HttpClient _clienteHttp;

        public ProveedorApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        // Pide al API la lista de proveedores.
        public async Task<List<ProveedorViewModel>> ListarAsync()
        {
            try
            {
                var proveedores = await _clienteHttp.GetFromJsonAsync<List<ProveedorViewModel>>("api/Proveedor/Listar");
                return proveedores ?? new List<ProveedorViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ListarAsync

        // Envía el proveedor al API; si lo rechaza, lanza el motivo.
        public async Task<bool> RegistrarAsync(ProveedorViewModel proveedor)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Proveedor/Registrar", proveedor);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                // Lee el mensaje enviado por el API en BadRequest(ex.Message)
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo registrar el proveedor." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }

        public async Task<List<ProveedorViewModel>> BuscarAsync(string texto, string criterio)
        {
            try
            {
                // Se envían ambos parámetros en la URL
                var respuesta = await _clienteHttp.GetAsync($"api/Proveedor/Buscar?texto={texto}&criterio={criterio}");

                if (!respuesta.IsSuccessStatusCode)
                {
                    string motivo = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception(motivo);
                }

                var resultado = await respuesta.Content.ReadFromJsonAsync<List<ProveedorViewModel>>();
                return resultado ?? new List<ProveedorViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }

        public async Task<ProveedorViewModel?> ObtenerAsync(Guid id)
        {
            try
            {
                var proveedor = await _clienteHttp.GetFromJsonAsync<ProveedorViewModel>($"api/Proveedor/Obtener/{id}");
                return proveedor;
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }//fn 

        public async Task<bool> EditarAsync(ProveedorViewModel proveedor)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/Proveedor/Editar/{proveedor.Id}", proveedor);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                // Lee el mensaje claro que enviamos de negocios
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo actualizar el proveedor." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }//fn

        public async Task<bool> EliminarAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/Proveedor/Eliminar/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                // Lee el mensaje de regla de negocio enviado desde la API
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo eliminar el proveedor." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn
    }// fin class
}
