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

        public async Task<Dictionary<int, string>> ObtenerCategoriasProductoAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/CategoriasProducto");
        }

        public async Task<Dictionary<int, string>> ObtenerUnidadesMedidaAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/UnidadesMedida");
        }

        public async Task<Dictionary<int, string>> ObtenerTiposIdentificacionClienteAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/TiposIdentificacionCliente");
        }

        public async Task<Dictionary<int, string>> ObtenerTiposIdentificacionEmpleadoAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/TiposIdentificacionEmpleado");
        }

        public async Task<Dictionary<int, string>> ObtenerAreasTrabajoAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/AreasTrabajo");
        }

        public async Task<Dictionary<int, string>> ObtenerPuestosAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/Puestos");
        }

        public async Task<Dictionary<int, string>> ObtenerTurnosAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/Turnos");
        }

        public async Task<Dictionary<string, List<string>>> ObtenerMapeoAreaPuestoAsync()
        {
            try
            {
                var mapeo = await _clienteHttp.GetFromJsonAsync<Dictionary<string, List<string>>>("api/Catalogos/MapeoAreaPuesto");
                return mapeo ?? new Dictionary<string, List<string>>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servicio. Intente más tarde.");
            }
        }

        public async Task<Dictionary<int, string>> ObtenerMetodosPagoAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/MetodosPago");
        }

        public async Task<Dictionary<int, string>> ObtenerEstadosFacturaAsync()
        {
            return await ObtenerCatalogoAsync("api/Catalogos/EstadosFactura");
        }

        #region Metodos Internos
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
