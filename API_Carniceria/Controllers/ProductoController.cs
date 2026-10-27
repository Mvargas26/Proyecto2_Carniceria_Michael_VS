using API_Carniceria.Entidades;
using API_Carniceria.Negocios.ProductoNegocios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Carniceria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoNegocios _productoNegocios;

        public ProductoController(IProductoNegocios productoNegocios)
        {
            _productoNegocios = productoNegocios;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Producto>>> Listar()
        {
            var productos = await _productoNegocios.ObtenerTodosAsync();
            return Ok(productos);
        } // fn

        [HttpGet("Obtener/{id}")]
        public async Task<ActionResult<Producto>> Obtener(Guid id)
        {
            var producto = await _productoNegocios.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return NotFound("No se encontró el producto.");
            }

            return Ok(producto);
        } // fn

        [HttpGet("Buscar")]
        public async Task<ActionResult<IEnumerable<Producto>>> Buscar(string texto, string criterio)
        {
            var productos = await _productoNegocios.BuscarAsync(texto, criterio);
            return Ok(productos);
        } // fn

        [HttpPost("Registrar")]
        public async Task<ActionResult<Producto>> Registrar([FromBody] Producto producto)
        {
            try
            {
                var creado = await _productoNegocios.CrearAsync(producto);
                return Ok(creado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // fn

        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> Editar(Guid id, [FromBody] Producto producto)
        {
            try
            {
                producto.Id = id;
                bool actualizado = await _productoNegocios.ActualizarAsync(producto);

                if (!actualizado)
                {
                    return NotFound("No se encontró el producto.");
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // fn

        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Eliminar(Guid id)
        {
            try
            {
                bool eliminado = await _productoNegocios.EliminarAsync(id);

                if (!eliminado)
                {
                    return NotFound("No se encontró el producto.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // fn

    }//fin class
}//fn space
