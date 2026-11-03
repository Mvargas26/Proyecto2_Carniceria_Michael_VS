using API_Carniceria.Entidades;
using API_Carniceria.Negocios.ClienteNegocios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteNegocios _clienteNegocios;

        public ClienteController(IClienteNegocios clienteNegocios)
        {
            _clienteNegocios = clienteNegocios;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _clienteNegocios.ObtenerTodosAsync();
            return Ok(clientes);
        }//fn

        [HttpGet("Obtener/{id:guid}")]
        public async Task<IActionResult> Obtener(Guid id)
        {
            var cliente = await _clienteNegocios.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                return NotFound("No se encontró la persona cliente solicitada.");
            }

            return Ok(cliente);
        }//fn

        [HttpGet("Buscar/{identificacion}")]
        public async Task<IActionResult> Buscar(string identificacion)
        {
            var cliente = await _clienteNegocios.BuscarPorIdentificacionAsync(identificacion);

            if (cliente == null)
            {
                return NotFound("No se encontró ninguna persona cliente con la identificación ingresada.");
            }

            return Ok(cliente);
        }//fn

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] Cliente cliente)
        {
            try
            {
                var nuevoCliente = await _clienteNegocios.CrearAsync(cliente);
                return Ok(nuevoCliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn

        [HttpPut("Editar/{id:guid}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("El identificador de la ruta no coincide con el del cliente.");
            }

            try
            {
                bool actualizado = await _clienteNegocios.ActualizarAsync(cliente);

                if (!actualizado)
                {
                    return NotFound("No se encontró la persona cliente para actualizar.");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn

        [HttpDelete("Eliminar/{id:guid}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                bool eliminado = await _clienteNegocios.EliminarAsync(id);

                if (!eliminado)
                {
                    return NotFound("No se encontró la persona cliente para eliminar.");
                }

                return Ok(new { mensaje = "Cliente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn
    
    }//fn class
}// fn space
