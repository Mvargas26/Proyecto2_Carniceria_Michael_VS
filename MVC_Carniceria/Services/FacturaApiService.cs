using System.Net;
using MVC_Carniceria.Models;

namespace MVC_Carniceria.Services
{
    public class FacturaApiService
    {
        private readonly HttpClient _clienteHttp;

        public FacturaApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<List<FacturaViewModel>> ListarAsync()
        {
            try
            {
                var facturas = await _clienteHttp.GetFromJsonAsync<List<FacturaViewModel>>("api/Factura/Listar");
                return facturas ?? new List<FacturaViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ListarAsync

        public async Task<FacturaViewModel?> ObtenerAsync(Guid id)
        {
            try
            {
                var factura = await _clienteHttp.GetFromJsonAsync<FacturaViewModel>($"api/Factura/Obtener/{id}");
                return factura;
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ObtenerAsync

        public async Task<List<FacturaViewModel>> BuscarAsync(string criterio)
        {
            try
            {
                var respuesta = await _clienteHttp.GetAsync($"api/Factura/Buscar/{Uri.EscapeDataString(criterio.Trim())}");

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<FacturaViewModel>();
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    string motivo = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception(string.IsNullOrWhiteSpace(motivo) ? "No se pudo realizar la búsqueda." : motivo);
                }

                var resultados = await respuesta.Content.ReadFromJsonAsync<List<FacturaViewModel>>();
                return resultados ?? new List<FacturaViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin BuscarAsync

        public async Task<bool> CrearAsync(FacturaViewModel factura)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Factura/Crear", factura);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo registrar la factura." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin CrearAsync

        public async Task<bool> ActualizarAsync(FacturaViewModel factura)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync("api/Factura/Actualizar", factura);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo actualizar la factura." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ActualizarAsync

        public async Task<bool> EliminarAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/Factura/Eliminar/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo eliminar la factura." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin EliminarAsync

        public async Task<bool> PagarAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsync($"api/Factura/Pagar/{id}", null);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo procesar el pago de la factura." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin PagarAsync

        public async Task<bool> AnularAsync(Guid id)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsync($"api/Factura/Anular/{id}", null);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo anular la factura." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin AnularAsync
    } // fin class
}