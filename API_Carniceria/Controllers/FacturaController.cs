using API_Carniceria.Entidades;
using API_Carniceria.Negocios.FacturaNegocios;
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

        [HttpGet("Buscar/{criterio}")]
        public async Task<IActionResult> Buscar(string criterio)
        {
            try
            {
                var resultados = await _facturaNegocios.BuscarAsync(criterio);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }//fn buscar

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

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] Factura factura)
        {
            try
            {
                var facturaActualizada = await _facturaNegocios.ActualizarAsync(factura);
                return Ok(facturaActualizada);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn actualizar

        [HttpDelete("Eliminar/{id:guid}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var eliminado = await _facturaNegocios.EliminarAsync(id);
                if (!eliminado)
                {
                    return NotFound("No se encontró la factura a eliminar.");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn eliminar

        [HttpPut("Pagar/{id:guid}")]
        public async Task<IActionResult> Pagar(Guid id)
        {
            try
            {
                var facturaPagada = await _facturaNegocios.PagarAsync(id);
                return Ok(facturaPagada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn pagar

        [HttpPut("Anular/{id:guid}")]
        public async Task<IActionResult> Anular(Guid id)
        {
            try
            {
                var facturaAnulada = await _facturaNegocios.AnularAsync(id);
                return Ok(facturaAnulada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn anular

    }//fn class
}//fn space