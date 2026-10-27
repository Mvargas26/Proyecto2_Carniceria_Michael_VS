using MVC_Carniceria.Models;

namespace MVC_Carniceria.Services
{
    public class ProductoApiService
    {
        private readonly HttpClient _clienteHttp;

        public ProductoApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        } // fn

        public async Task<List<ProductoViewModel>> ObtenerTodosAsync()
        {
            try
            {
                var productos = await _clienteHttp.GetFromJsonAsync<List<ProductoViewModel>>("api/Producto/Listar");
                return productos ?? new List<ProductoViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

        public async Task<ProductoViewModel?> ObtenerPorIdAsync(Guid id)
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<ProductoViewModel>($"api/Producto/Obtener/{id}");
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

        public async Task<List<ProductoViewModel>> BuscarAsync(string texto, string criterio)
        {
            try
            {
                string url = $"api/Producto/Buscar?texto={Uri.EscapeDataString(texto)}&criterio={Uri.EscapeDataString(criterio)}";
                var productos = await _clienteHttp.GetFromJsonAsync<List<ProductoViewModel>>(url);
                return productos ?? new List<ProductoViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

        public async Task<bool> RegistrarAsync(ProductoViewModel producto)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Producto/Registrar", producto);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo registrar el producto." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

        public async Task<bool> EditarAsync(ProductoViewModel producto)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/Producto/Editar/{producto.Id}", producto);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo actualizar el producto." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

        public async Task<bool> EliminarAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/Producto/Eliminar/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo eliminar el producto." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fn

    } // fn class
}
