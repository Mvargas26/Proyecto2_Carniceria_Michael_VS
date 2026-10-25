using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Carniceria.Entidades;
using API_Carniceria.Services.Proveedor_Service;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        // variables internas
        private readonly IProveedorNegocios _proveedorNegocios;

        //constru
        public ProveedorController(IProveedorNegocios proveedorService)
        {
            _proveedorNegocios = proveedorService;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Listar()
        {
            var proveedores = await _proveedorNegocios.ObtenerTodosAsync();
            return Ok(proveedores);
        }//fn


        [HttpGet("Detalles/{id}")]
        public async Task<ActionResult<Proveedor>> Detalles(Guid id)
        {
            var proveedor = await _proveedorNegocios.ObtenerPorIdAsync(id);

            if (proveedor == null)
            {
                return NotFound("No se encontró el proveedor.");
            }

            return Ok(proveedor);
        }//fn

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Buscar(string texto, string criterio)
        {
            var proveedores = await _proveedorNegocios.BuscarAsync(texto, criterio);
            return Ok(proveedores);
        }//fn

        [HttpPost("Registrar")]
        public async Task<ActionResult<Proveedor>> Registrar([FromBody] Proveedor proveedor)
        {
            try
            {
                var creado = await _proveedorNegocios.CrearAsync(proveedor);
                return Ok(creado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn

        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> Editar(Guid id, [FromBody] Proveedor proveedor)
        {
            try
            {
                proveedor.Id = id;
                bool actualizado = await _proveedorNegocios.ActualizarAsync(proveedor);

                if (!actualizado)
                {
                    return NotFound("No se encontró el proveedor.");
                }

                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn



    }//fn class
}//fn space
