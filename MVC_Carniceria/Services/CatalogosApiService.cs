namespace MVC_Carniceria.Services
{
    public class CatalogosApiService 
    {
        private readonly HttpClient _clienteHttp;

        public CatalogosApiService(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<Dictionary<int, string>> ObtenerTiposSuministroAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/TiposSuministro");
        }

        public async Task<Dictionary<int, string>> ObtenerFrecuenciasEntregaAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/FrecuenciasEntrega");
        }

        public async Task<Dictionary<int, string>> ObtenerTiposIdentificacionProveedorAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/TiposIdentificacionProveedor");
        }


        #region Metodos Internos

        // Pide un catálogo al API; si falla, lanza un mensaje claro.
        private async Task<Dictionary<int, string>> ObtenerCatalogoAsync(string ruta)
        {
            try
            {
                var catalogo = await _clienteHttp.GetFromJsonAsync<Dictionary<int, string>>(ruta);
                return catalogo ?? new Dictionary<int, string>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }//fn ObtenerCatalogoAsync

        #endregion

    }//fn class
}//fn space
