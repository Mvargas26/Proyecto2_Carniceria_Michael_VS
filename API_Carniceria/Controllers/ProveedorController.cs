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

        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Listar()
        {
            var proveedores = await _proveedorService.ObtenerTodosAsync();
            return Ok(proveedores);
        }



    }//fn class
}//fn space
