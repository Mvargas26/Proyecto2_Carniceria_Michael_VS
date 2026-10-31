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
            await CargarCatalogosYProveedoresParaSelectAsync();
            return View(new ProductoViewModel());
        } // fn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarProducto(ProductoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarCatalogosYProveedoresParaSelectAsync();
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
                await CargarCatalogosYProveedoresParaSelectAsync();
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

                await CargarCatalogosYProveedoresParaSelectAsync();
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
                await CargarCatalogosYProveedoresParaSelectAsync();
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
                await CargarCatalogosYProveedoresParaSelectAsync();
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
            // Cargamos los catálogos para que la tabla pueda traducir los IDs a texto
            await CargarCatalogosParaListasAsync();

            // Guardamos los valores para que el formulario los conserve escritos
            ViewBag.Texto = texto;
            ViewBag.Criterio = criterio;

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
        } // fn

        #region MetodosInternos

        private async Task CargarCatalogosYProveedoresParaSelectAsync()
        {
            try
            {
                // 1. Proveedores
                var proveedores = await _proveedorApiService.ListarAsync();
                ViewBag.Proveedores = new SelectList(proveedores, "Id", "RazonSocial");

                // 2. Catálogos desde el API
                var categorias = await _catalogosApiService.ObtenerCategoriasProductoAsync();
                ViewBag.Categorias = new SelectList(categorias, "Key", "Value");

                var unidadesMedida = await _catalogosApiService.ObtenerUnidadesMedidaAsync();
                ViewBag.UnidadesMedida = new SelectList(unidadesMedida, "Key", "Value");
            }
            catch
            {
                ViewBag.Proveedores = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Categorias = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.UnidadesMedida = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        } // fn

        private async Task CargarCatalogosParaListasAsync()
        {
            try
            {
                ViewBag.Categorias = await _catalogosApiService.ObtenerCategoriasProductoAsync();
                ViewBag.UnidadesMedida = await _catalogosApiService.ObtenerUnidadesMedidaAsync();
            }
            catch
            {
                ViewBag.Categorias = new Dictionary<int, string>();
                ViewBag.UnidadesMedida = new Dictionary<int, string>();
            }
        } // fn
        #endregion

    } // fn class
}//fn space
