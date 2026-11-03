using MVC_Carniceria.Models;
using System.Net;

namespace MVC_Carniceria.Services
{
    public class ClienteApiService
    {
        private readonly HttpClient _clienteHttp;

        public ClienteApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<List<ClienteViewModel>> ListarAsync()
        {
            try
            {
                var clientes = await _clienteHttp.GetFromJsonAsync<List<ClienteViewModel>>("api/Cliente/Listar");
                return clientes ?? new List<ClienteViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ListarAsync

        public async Task<bool> RegistrarAsync(ClienteViewModel cliente)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Cliente/Registrar", cliente);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo registrar la persona cliente." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin RegistrarAsync

        public async Task<ClienteViewModel?> BuscarPorIdentificacionAsync(string identificacion)
        {
            try
            {
                var respuesta = await _clienteHttp.GetAsync($"api/Cliente/Buscar/{Uri.EscapeDataString(identificacion.Trim())}");

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    string motivo = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception(string.IsNullOrWhiteSpace(motivo) ? "No se pudo realizar la búsqueda." : motivo);
                }

                return await respuesta.Content.ReadFromJsonAsync<ClienteViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin BuscarPorIdentificacionAsync

        public async Task<ClienteViewModel?> ObtenerAsync(Guid id)
        {
            try
            {
                var cliente = await _clienteHttp.GetFromJsonAsync<ClienteViewModel>($"api/Cliente/Obtener/{id}");
                return cliente;
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ObtenerAsync

        public async Task<bool> EditarAsync(ClienteViewModel cliente)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/Cliente/Editar/{cliente.Id}", cliente);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo actualizar la persona cliente." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin EditarAsync

        public async Task<bool> EliminarAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/Cliente/Eliminar/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo eliminar la persona cliente." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin EliminarAsync
    } // fin class
}//fn space
