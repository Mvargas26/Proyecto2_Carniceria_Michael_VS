using API_Carniceria.Entidades;
using API_Carniceria.Negocios.FacturaNegocios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaNegocios _facturaNegocios;

        public FacturaController(IFacturaNegocios facturaNegocios)
        {
            _facturaNegocios = facturaNegocios;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var facturas = await _facturaNegocios.ObtenerTodasAsync();
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }//fn listar

        [HttpGet("Obtener/{id:guid}")]
        public async Task<IActionResult> Obtener(Guid id)
        {
            try
            {
                var factura = await _facturaNegocios.ObtenerPorIdAsync(id);
                if (factura == null)
                {
                    return NotFound("No se encontró la factura solicitada.");
                }

                return Ok(factura);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }//fn obtener

        [HttpGet("Buscar/{numeroFactura:int}")]
        public async Task<IActionResult> BuscarPorNumero(int numeroFactura)
        {
            try
            {
                var factura = await _facturaNegocios.ObtenerPorNumeroAsync(numeroFactura);
                if (factura == null)
                {
                    return NotFound("No se encontró ninguna factura con el número ingresado.");
                }

                return Ok(factura);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }//fn buscar por numero

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Factura factura)
        {
            try
            {
                var facturaCreada = await _facturaNegocios.CrearAsync(factura);
                return CreatedAtAction(nameof(Obtener), new { id = facturaCreada.Id }, facturaCreada);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn crear
    
    }//fn class
}//fn space
