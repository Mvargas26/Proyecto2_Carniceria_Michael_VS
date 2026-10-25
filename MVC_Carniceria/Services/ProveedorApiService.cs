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

        // Pide al API un proveedor por su ID.
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
        } // fin ObtenerAsync

        // Envía el proveedor al API; si lo rechaza, lanza el motivo.
        public async Task RegistrarAsync(ProveedorViewModel proveedor)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Proveedor/Registrar", proveedor);

                if (!respuesta.IsSuccessStatusCode)
                {
                    string motivo = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception(motivo);
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }//fn


    }// fin class
}
