using System.Net;
using MVC_Carniceria.Models;

namespace MVC_Carniceria.Services
{
    public class EmpleadoApiService
    {
        private readonly HttpClient _clienteHttp;

        public EmpleadoApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<List<EmpleadoViewModel>> ListarAsync()
        {
            try
            {
                var empleados = await _clienteHttp.GetFromJsonAsync<List<EmpleadoViewModel>>("api/Empleado/Listar");
                return empleados ?? new List<EmpleadoViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ListarAsync

        public async Task<bool> RegistrarAsync(EmpleadoViewModel empleado)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/Empleado/Registrar", empleado);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo registrar la persona empleada." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin RegistrarAsync

        public async Task<EmpleadoViewModel?> BuscarPorIdentificacionAsync(string identificacion)
        {
            try
            {
                var respuesta = await _clienteHttp.GetAsync($"api/Empleado/Buscar/{Uri.EscapeDataString(identificacion.Trim())}");

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    string motivo = await respuesta.Content.ReadAsStringAsync();
                    throw new Exception(string.IsNullOrWhiteSpace(motivo) ? "No se pudo realizar la búsqueda." : motivo);
                }

                return await respuesta.Content.ReadFromJsonAsync<EmpleadoViewModel>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin BuscarPorIdentificacionAsync

        public async Task<EmpleadoViewModel?> ObtenerAsync(Guid id)
        {
            try
            {
                var empleado = await _clienteHttp.GetFromJsonAsync<EmpleadoViewModel>($"api/Empleado/Obtener/{id}");
                return empleado;
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin ObtenerAsync

        public async Task<bool> EditarAsync(EmpleadoViewModel empleado)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/Empleado/Editar/{empleado.Id}", empleado);

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo actualizar la persona empleada." : mensajeError);
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
                var respuesta = await _clienteHttp.DeleteAsync($"api/Empleado/Eliminar/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }

                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(mensajeError) ? "No se pudo eliminar la persona empleada." : mensajeError);
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        } // fin EliminarAsync
    } // fin class
}//fn space
