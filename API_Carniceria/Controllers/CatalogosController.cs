using API_Carniceria.Enums;
using API_Carniceria.Enums.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        // Devolvemos los tipos de suministro como número y nombre visible.
        [HttpGet("TiposSuministro")]
        public ActionResult<Dictionary<int, string>> ObtenerTiposSuministro()
        {
            Dictionary<int, string> tiposSuministro = new();

            foreach (TipoSuministro tipo in Enum.GetValues<TipoSuministro>())
            {
                int numero = (int)tipo;
                string nombre = tipo.ObtenerNombreVisible();

                tiposSuministro.Add(numero, nombre);
            }

            return Ok(tiposSuministro);
        }//fn ObtenerTiposSuministro

        // Devolvemos las frecuencias de entrega como número y nombre visible.

        [HttpGet("FrecuenciasEntrega")]
        public ActionResult<Dictionary<int, string>> ObtenerFrecuenciasEntrega()
        {
            Dictionary<int, string> frecuencias = new();

            foreach (FrecuenciaEntrega frecuencia in Enum.GetValues<FrecuenciaEntrega>())
            {
                int numero = (int)frecuencia;
                string nombre = frecuencia.ObtenerNombreVisible();

                frecuencias.Add(numero, nombre);
            }

            return Ok(frecuencias);
        }// ObtenerFrecuenciasEntrega

        // Devolvemos los tipos de identificación que aplican a Proveedor.

        [HttpGet("TiposIdentificacionProveedor")]
        public ActionResult<Dictionary<int, string>> ObtenerTiposIdentificacionProveedor()
        {
            List<TipoIdentificacion> tiposPermitidos = new()
            {
                TipoIdentificacion.CedulaNacional,
                TipoIdentificacion.CedulaJuridica,
                TipoIdentificacion.Dimex
            };

            Dictionary<int, string> tiposIdentificacion = new();

            foreach (TipoIdentificacion tipo in tiposPermitidos)
            {
                int numero = (int)tipo;
                string nombre = tipo.ObtenerNombreVisible();

                tiposIdentificacion.Add(numero, nombre);
            }

            return Ok(tiposIdentificacion);
        }// fn ObtenerTiposIdentificacionProveedor

        [HttpGet("CategoriasProducto")]
        public ActionResult<Dictionary<int, string>> ObtenerCategoriasProducto()
        {
            var categorias = Enum.GetValues(typeof(Categoria))
                .Cast<Categoria>()
                .ToDictionary(c => (int)c, c => c.ToString());

            return Ok(categorias);
        }

        [HttpGet("UnidadesMedida")]
        public ActionResult<Dictionary<int, string>> ObtenerUnidadesMedida()
        {
            var unidades = Enum.GetValues(typeof(UnidadMedida))
                .Cast<UnidadMedida>()
                .ToDictionary(u => (int)u, u => u.ToString());

            return Ok(unidades);
        }

    }//fin class
}//fn space
