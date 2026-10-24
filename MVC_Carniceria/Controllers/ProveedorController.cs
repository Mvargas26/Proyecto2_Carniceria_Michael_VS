using Microsoft.AspNetCore.Mvc;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;

namespace MVC_Carniceria.Controllers
{
    public class ProveedorController : Controller
    {

        private readonly ProveedorApiService _proveedorApiService;

        //constru
        public ProveedorController(ProveedorApiService proveedorApiService)
        {
            _proveedorApiService = proveedorApiService;
        }

        public async Task<IActionResult> ListarProveedores()
        {
            try
            {
                var proveedores = await _proveedorApiService.ListarAsync();
                return View(proveedores);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<ProveedorViewModel>());
            }
        }






    }//fin class
}//fn space
