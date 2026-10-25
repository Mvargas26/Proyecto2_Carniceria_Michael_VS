using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> RegistrarProveedor()
        {
            try
            {
                await CargarListasDesplegables();
                return View(new ProveedorViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarProveedores", new List<ProveedorViewModel>());
            }
        }//fn

        [HttpPost]
        public async Task<IActionResult> RegistrarProveedor(ProveedorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _proveedorApiService.RegistrarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarProveedores");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            try
            {
                await CargarListasDesplegables();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarProveedores", new List<ProveedorViewModel>());
            }
        } // fn




        #region MetodosInternos

        // Llena los selects con los catálogos que entrega el API.
        private async Task CargarListasDesplegables()
        {
            var tiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionProveedorAsync();
            var tiposSuministro = await _catalogosApiService.ObtenerTiposSuministroAsync();
            var frecuenciasEntrega = await _catalogosApiService.ObtenerFrecuenciasEntregaAsync();

            ViewBag.ListaTiposIdentificacion = new SelectList(tiposIdentificacion, "Key", "Value");
            ViewBag.ListaTiposSuministro = new SelectList(tiposSuministro, "Key", "Value");
            ViewBag.ListaFrecuenciasEntrega = new SelectList(frecuenciasEntrega, "Key", "Value");
        }

        #endregion


    }//fin class
}//fn space
