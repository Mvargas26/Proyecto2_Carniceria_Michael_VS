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

        [HttpGet("TiposIdentificacionCliente")]
        public ActionResult<Dictionary<int, string>> ObtenerTiposIdentificacionCliente()
        {
            List<TipoIdentificacion> tiposPermitidos = new()
            {
                TipoIdentificacion.CedulaNacional,
                TipoIdentificacion.Dimex,
                TipoIdentificacion.Pasaporte,
                TipoIdentificacion.CedulaJuridica
            };

            Dictionary<int, string> tiposIdentificacion = new();

            foreach (TipoIdentificacion tipo in tiposPermitidos)
            {
                int numero = (int)tipo;
                string nombre = tipo.ObtenerNombreVisible();

                tiposIdentificacion.Add(numero, nombre);
            }

            return Ok(tiposIdentificacion);
        } // fn ObtenerTiposIdentificacionCliente

        [HttpGet("TiposIdentificacionEmpleado")]
        public ActionResult<Dictionary<int, string>> ObtenerTiposIdentificacionEmpleado()
        {
            List<TipoIdentificacion> tiposPermitidos = new()
            {
                TipoIdentificacion.CedulaNacional,
                TipoIdentificacion.Dimex,
                TipoIdentificacion.Pasaporte
            };

            Dictionary<int, string> tipos = new();
            foreach (var tipo in tiposPermitidos)
            {
                tipos.Add((int)tipo, tipo.ObtenerNombreVisible());
            }

            return Ok(tipos);
        }//fn

        [HttpGet("AreasTrabajo")]
        public ActionResult<Dictionary<int, string>> ObtenerAreasTrabajo()
        {
            var areas = Enum.GetValues<AreaTrabajo>()
                .ToDictionary(a => (int)a, a => a.ObtenerNombreVisible());

            return Ok(areas);
        }

        [HttpGet("Puestos")]
        public ActionResult<Dictionary<int, string>> ObtenerPuestos()
        {
            var puestos = Enum.GetValues<Puesto>()
                .ToDictionary(p => (int)p, p => p.ObtenerNombreVisible());

            return Ok(puestos);
        }

        [HttpGet("Turnos")]
        public ActionResult<Dictionary<int, string>> ObtenerTurnos()
        {
            var turnos = Enum.GetValues<Turno>()
                .ToDictionary(t => (int)t, t => t.ObtenerNombreVisible());

            return Ok(turnos);
        }//fn

        [HttpGet("MapeoAreaPuesto")]
        public ActionResult<Dictionary<string, List<string>>> ObtenerMapeoAreaPuestoEndpoint()
        {
            var mapeo = new Dictionary<string, List<string>>();

            foreach (AreaTrabajo area in Enum.GetValues(typeof(AreaTrabajo)))
            {
                var puestosDelArea = Enum.GetValues(typeof(Puesto))
                    .Cast<Puesto>()
                    .Where(p => PuestoPerteneceAArea(area, p))
                    .Select(p => ((int)p).ToString())
                    .ToList();

                mapeo[((int)area).ToString()] = puestosDelArea;
            }

            return Ok(mapeo);
        }

        [HttpGet("MetodosPago")]
        public ActionResult<Dictionary<int, string>> ObtenerMetodosPago()
        {
            var metodos = Enum.GetValues<MetodoPago>()
                .ToDictionary(m => (int)m, m => m.ObtenerNombreVisible());

            return Ok(metodos);
        }

        [HttpGet("EstadosFactura")]
        public ActionResult<Dictionary<int, string>> ObtenerEstadosFactura()
        {
            var estados = Enum.GetValues<EstadoFactura>()
                .ToDictionary(e => (int)e, e => e.ToString());

            return Ok(estados);
        }

        #region MetodosAuxiliares

        private static bool PuestoPerteneceAArea(AreaTrabajo area, Puesto puesto)
        {
            return area switch
            {
                AreaTrabajo.Administracion => puesto is Puesto.Administrador or Puesto.AsistenteAdministrativo,
                AreaTrabajo.VentasYCaja => puesto is Puesto.Cajero or Puesto.Vendedor or Puesto.ServicioAlCliente,
                AreaTrabajo.CorteYPreparacion => puesto is Puesto.Carnicero or Puesto.Despostador or Puesto.Empacador,
                AreaTrabajo.BodegaEInventario => puesto is Puesto.Bodeguero or Puesto.AuxiliarDeInventario,
                AreaTrabajo.Distribucion => puesto is Puesto.Conductor or Puesto.AyudanteDeDistribucion,
                AreaTrabajo.Limpieza => puesto is Puesto.PersonalDeLimpieza,
                _ => false
            };
        }

        #endregion

    }//fin class
}//fn space