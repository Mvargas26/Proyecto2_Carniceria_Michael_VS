using API_Carniceria.Entidades;
using API_Carniceria.Negocios.EmpleadoNegocios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoNegocios _empleadoNegocios;

        public EmpleadoController(IEmpleadoNegocios empleadoNegocios)
        {
            _empleadoNegocios = empleadoNegocios;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            var empleados = await _empleadoNegocios.ObtenerTodosAsync();
            return Ok(empleados);
        }//fn

        [HttpGet("Obtener/{id:guid}")]
        public async Task<IActionResult> Obtener(Guid id)
        {
            var empleado = await _empleadoNegocios.ObtenerPorIdAsync(id);

            if (empleado == null)
            {
                return NotFound("No se encontró la persona empleada solicitada.");
            }

            return Ok(empleado);
        }//fn

        [HttpGet("Buscar/{identificacion}")]
        public async Task<IActionResult> Buscar(string identificacion)
        {
            var empleado = await _empleadoNegocios.BuscarPorIdentificacionAsync(identificacion);

            if (empleado == null)
            {
                return NotFound("No se encontró ninguna persona empleada con la identificación ingresada.");
            }

            return Ok(empleado);
        }//fn

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] Empleado empleado)
        {
            try
            {
                var nuevoEmpleado = await _empleadoNegocios.CrearAsync(empleado);
                return Ok(nuevoEmpleado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn

        [HttpPut("Editar/{id:guid}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return BadRequest("El identificador de la ruta no coincide con el del empleado.");
            }

            try
            {
                bool actualizado = await _empleadoNegocios.ActualizarAsync(empleado);

                if (!actualizado)
                {
                    return NotFound("No se encontró la persona empleada para actualizar.");
                }

                return Ok(empleado);
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
                bool eliminado = await _empleadoNegocios.EliminarAsync(id);

                if (!eliminado)
                {
                    return NotFound("No se encontró la persona empleada para eliminar.");
                }

                return Ok(new { mensaje = "Empleado eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }//fn

    }//fn class
}//fn space
