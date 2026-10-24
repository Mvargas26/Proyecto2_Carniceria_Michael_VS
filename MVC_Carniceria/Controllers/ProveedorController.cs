using Microsoft.AspNetCore.Mvc;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;

namespace MVC_Carniceria.Controllers
{
    public class ProveedorController : Controller
    {
        //variables internas
        private readonly ProveedorApiService _proveedorApiService;
        private readonly CatalogosApiService _catalogosApiService;


        //constru
        public ProveedorController(ProveedorApiService proveedorApiService, CatalogosApiService catalogosApiService)
        {
            _proveedorApiService = proveedorApiService;
            _catalogosApiService = catalogosApiService;
        }

        public async Task<IActionResult> ListarProveedores()
        {
            try
            {
                var proveedores = await _proveedorApiService.ListarAsync();

                //estos no pasan por negocios, porque solo lee los catalogos, no hay reglas de negocio, solo lectura de datos
                ViewBag.TiposSuministro = await _catalogosApiService.ObtenerTiposSuministroAsync();
                ViewBag.FrecuenciasEntrega = await _catalogosApiService.ObtenerFrecuenciasEntregaAsync();

                return View(proveedores);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<ProveedorViewModel>());
            }
        }//fn ListarProveedores






    }//fin class
}//fn space
