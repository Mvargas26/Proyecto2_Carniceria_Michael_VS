using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;

namespace MVC_Carniceria.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoApiService _productoApiService;
        private readonly ProveedorApiService _proveedorApiService;
        private readonly CatalogosApiService _catalogosApiService;

        public ProductoController(ProductoApiService productoApiService, ProveedorApiService proveedorApiService, CatalogosApiService catalogosApiService)
        {
            _productoApiService = productoApiService;
            _proveedorApiService = proveedorApiService;
            _catalogosApiService = catalogosApiService;
        } // fn

        [HttpGet]
        public async Task<IActionResult> ListarProductos()
        {
            try
            {
                var productos = await _productoApiService.ObtenerTodosAsync();
                var proveedores = await _proveedorApiService.ListarAsync();

                ViewBag.Categorias = await _catalogosApiService.ObtenerCategoriasProductoAsync();
                ViewBag.UnidadesMedida = await _catalogosApiService.ObtenerUnidadesMedidaAsync();

                var diccionarioProveedores = proveedores.ToDictionary(p => p.Id, p => p.RazonSocial);
                foreach (var prod in productos)
                {
                    if (prod.ProveedorId.HasValue && diccionarioProveedores.TryGetValue(prod.ProveedorId.Value, out var razonSocial))
                    {
                        prod.RazonSocialProveedor = razonSocial;
                    }
                    else
                    {
                        prod.RazonSocialProveedor = "No asignado";
                    }
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(new List<ProductoViewModel>());
            }
        }//fn

        [HttpGet]
        public async Task<IActionResult> RegistrarProducto()
        {
            await CargarProveedoresEnViewBagAsync();
            return View(new ProductoViewModel());
        } // fn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarProducto(ProductoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarProveedoresEnViewBagAsync();
                return View(modelo);
            }

            try
            {
                await _productoApiService.RegistrarAsync(modelo);
                TempData["MensajeExito"] = "Producto registrado exitosamente.";
                return RedirectToAction(nameof(ListarProductos));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarProveedoresEnViewBagAsync();
                return View(modelo);
            }
        } // fn

        [HttpGet]
        public async Task<IActionResult> EditarProducto(Guid id)
        {
            try
            {
                var producto = await _productoApiService.ObtenerPorIdAsync(id);

                if (producto == null)
                {
                    TempData["MensajeError"] = "No se encontró el producto solicitado.";
                    return RedirectToAction(nameof(ListarProductos));
                }

                await CargarProveedoresEnViewBagAsync();
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction(nameof(ListarProductos));
            }
        } // fn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(ProductoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarProveedoresEnViewBagAsync();
                return View(modelo);
            }

            try
            {
                await _productoApiService.EditarAsync(modelo);
                TempData["MensajeExito"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(ListarProductos));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarProveedoresEnViewBagAsync();
                return View(modelo);
            }
        } // fn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto(Guid id)
        {
            try
            {
                await _productoApiService.EliminarAsync(id);
                TempData["MensajeExito"] = "Producto eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction(nameof(ListarProductos));
        } // fn

        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string? texto, string? criterio)
        {
            if (string.IsNullOrWhiteSpace(texto) || string.IsNullOrWhiteSpace(criterio))
            {
                return View(new List<ProductoViewModel>());
            }

            try
            {
                var productos = await _productoApiService.BuscarAsync(texto, criterio);
                var proveedores = await _proveedorApiService.ListarAsync();
                var diccionarioProveedores = proveedores.ToDictionary(p => p.Id, p => p.RazonSocial);

                foreach (var prod in productos)
                {
                    if (prod.ProveedorId.HasValue && diccionarioProveedores.TryGetValue(prod.ProveedorId.Value, out var razonSocial))
                    {
                        prod.RazonSocialProveedor = razonSocial;
                    }
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(new List<ProductoViewModel>());
            }
        } // fn

        #region MetodosInternos

        private async Task CargarProveedoresEnViewBagAsync()
        {
            try
            {
                var proveedores = await _proveedorApiService.ListarAsync();
                ViewBag.Proveedores = new SelectList(proveedores, "Id", "RazonSocial");
            }
            catch
            {
                ViewBag.Proveedores = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        } // fn

        #endregion

    } // fn class
}//fn space
