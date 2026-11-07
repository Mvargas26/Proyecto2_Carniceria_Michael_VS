using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;
using System.Text.Json;

namespace MVC_Carniceria.Controllers
{
    public class FacturaController : Controller
    {
        private readonly FacturaApiService _facturaApiService;
        private readonly ClienteApiService _clienteApiService;
        private readonly EmpleadoApiService _empleadoApiService;
        private readonly ProductoApiService _productoApiService;
        private readonly CatalogosApiService _catalogosApiService;

        //constru
        public FacturaController(
            FacturaApiService facturaApiService,
            ClienteApiService clienteApiService,
            EmpleadoApiService empleadoApiService,
            ProductoApiService productoApiService,
            CatalogosApiService catalogosApiService)
        {
            _facturaApiService = facturaApiService;
            _clienteApiService = clienteApiService;
            _empleadoApiService = empleadoApiService;
            _productoApiService = productoApiService;
            _catalogosApiService = catalogosApiService;
        }//fn constru

        [HttpGet]
        public async Task<IActionResult> ListarFacturas()
        {
            try
            {
                var facturas = await _facturaApiService.ListarAsync();

                ViewBag.MetodosPago = await _catalogosApiService.ObtenerMetodosPagoAsync();
                ViewBag.EstadosFactura = await _catalogosApiService.ObtenerEstadosFacturaAsync();

                return View(facturas);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<FacturaViewModel>());
            }
        }//fn ListarFacturas

        [HttpGet]
        public async Task<IActionResult> DetallesFactura(Guid id)
        {
            try
            {
                var factura = await _facturaApiService.ObtenerAsync(id);

                if (factura == null)
                {
                    ViewBag.MensajeError = "No se encontró la factura especificada.";
                    return View("ListarFacturas", new List<FacturaViewModel>());
                }

                ViewBag.MetodosPago = await _catalogosApiService.ObtenerMetodosPagoAsync();
                ViewBag.EstadosFactura = await _catalogosApiService.ObtenerEstadosFacturaAsync();

                return View(factura);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarFacturas", new List<FacturaViewModel>());
            }
        }//fn DetallesFactura

        [HttpGet]
        public async Task<IActionResult> RegistrarFactura()
        {
            try
            {
                await CargarListasDesplegables();
                return View(new FacturaViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarFacturas", new List<FacturaViewModel>());
            }
        }//fn RegistrarFactura

        [HttpPost]
        public async Task<IActionResult> RegistrarFactura(FacturaViewModel viewModel)
        {
            if (viewModel.Detalles == null || !viewModel.Detalles.Any())
            {
                ModelState.AddModelError("", "La factura debe contener al menos un producto en el detalle.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _facturaApiService.CrearAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarFacturas");
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
                return View("ListarFacturas", new List<FacturaViewModel>());
            }
        }//fn RegistrarFactura

        [HttpGet]
        public IActionResult BuscarFactura()
        {
            return View(new List<FacturaViewModel>());
        }//fn BuscarFactura

        [HttpPost]
        public async Task<IActionResult> BuscarFactura(string criterio, string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                ViewBag.MensajeInformativo = "Debe ingresar el texto a buscar.";
                return View(new List<FacturaViewModel>());
            }

            try
            {
                var facturas = await _facturaApiService.BuscarAsync(texto);

                if (facturas == null || !facturas.Any())
                {
                    ViewBag.MensajeInformativo = "No se encontraron facturas con el criterio de búsqueda ingresado.";
                }

                ViewBag.MetodosPago = await _catalogosApiService.ObtenerMetodosPagoAsync();
                ViewBag.EstadosFactura = await _catalogosApiService.ObtenerEstadosFacturaAsync();

                return View(facturas ?? new List<FacturaViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<FacturaViewModel>());
            }
        }//fn BuscarFactura

        [HttpGet]
        public async Task<IActionResult> EditarFactura(Guid id)
        {
            try
            {
                var factura = await _facturaApiService.ObtenerAsync(id);

                if (factura == null)
                {
                    ViewBag.MensajeError = "No se encontró la factura especificada.";
                    return View("ListarFacturas", new List<FacturaViewModel>());
                }

                if (factura.Estado == 3)
                {
                    TempData["MensajeError"] = "Una factura en estado Anulada no puede ser modificada.";
                    return RedirectToAction("ListarFacturas");
                }

                await CargarListasDesplegables();
                return View(factura);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarFacturas", new List<FacturaViewModel>());
            }
        }//fn EditarFactura

        [HttpPost]
        public async Task<IActionResult> EditarFactura(Guid id, FacturaViewModel viewModel)
        {
            if (viewModel.Detalles == null || !viewModel.Detalles.Any())
            {
                ModelState.AddModelError("", "La factura debe contener al menos un producto en el detalle.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _facturaApiService.ActualizarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarFacturas");
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
                return View("ListarFacturas", new List<FacturaViewModel>());
            }
        }//fn EditarFactura

        [HttpPost]
        public async Task<IActionResult> EliminarFactura(Guid id)
        {
            try
            {
                await _facturaApiService.EliminarAsync(id);
                TempData["MensajeExito"] = "Factura eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ListarFacturas");
        }//fn EliminarFactura

        [HttpPost]
        public async Task<IActionResult> PagarFactura(Guid id)
        {
            try
            {
                await _facturaApiService.PagarAsync(id);
                TempData["MensajeExito"] = "Factura pagada correctamente y existencias rebajadas del inventario.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ListarFacturas");
        }//fn PagarFactura

        [HttpPost]
        public async Task<IActionResult> AnularFactura(Guid id)
        {
            try
            {
                await _facturaApiService.AnularAsync(id);
                TempData["MensajeExito"] = "Factura anulada correctamente y existencias reintegradas al inventario.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ListarFacturas");
        }//fn AnularFactura

        #region MetodosInternos

        private async Task CargarListasDesplegables()
        {
            var clientes = await _clienteApiService.ListarAsync();
            var empleados = await _empleadoApiService.ListarAsync();
            var metodosPago = await _catalogosApiService.ObtenerMetodosPagoAsync();
            var productos = await _productoApiService.ObtenerTodosAsync();

            ViewBag.ListaClientes = new SelectList(clientes, "Id", "NombreCompleto");
            ViewBag.ListaEmpleados = new SelectList(empleados, "Id", "NombreCompleto");
            ViewBag.ListaMetodosPago = new SelectList(metodosPago, "Key", "Value");
            ViewBag.ListaProductos = new SelectList(productos, "Id", "NombreProducto");
            ViewBag.ProductosJson = JsonSerializer.Serialize(productos);

        }//fn CargarListasDesplegables

        #endregion

    }//fn class
}//fn space

