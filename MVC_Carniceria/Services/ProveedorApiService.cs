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


    }// fin class
}
