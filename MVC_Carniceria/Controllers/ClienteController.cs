using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;

namespace MVC_Carniceria.Controllers
{
    public class ClienteController : Controller
    {
        // Variables internas
        private readonly ClienteApiService _clienteApiService;
        private readonly CatalogosApiService _catalogosApiService;

        // Constru
        public ClienteController(ClienteApiService clienteApiService, CatalogosApiService catalogosApiService)
        {
            _clienteApiService = clienteApiService;
            _catalogosApiService = catalogosApiService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var clientes = await _clienteApiService.ListarAsync();

                ViewBag.TiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionClienteAsync();

                return View(clientes);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<ClienteViewModel>());
            }
        } // fn ListarClientes

        [HttpGet]
        public async Task<IActionResult> RegistrarCliente()
        {
            try
            {
                await CargarListasDesplegables();
                return View(new ClienteViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarClientes", new List<ClienteViewModel>());
            }
        } // fn

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(ClienteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteApiService.RegistrarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarClientes");
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
                return View("ListarClientes", new List<ClienteViewModel>());
            }
        } // fn

        [HttpGet]
        public IActionResult BuscarCliente()
        {
            return View(new List<ClienteViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> BuscarCliente(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                ViewBag.MensajeInformativo = "Debe ingresar el número de identificación que desea consultar.";
                return View(new List<ClienteViewModel>());
            }

            try
            {
                var cliente = await _clienteApiService.BuscarPorIdentificacionAsync(identificacion);
                var lista = new List<ClienteViewModel>();

                if (cliente == null)
                {
                    ViewBag.MensajeInformativo = "No se encontró ninguna persona cliente con la identificación ingresada.";
                }
                else
                {
                    lista.Add(cliente);
                }

                ViewBag.TiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionClienteAsync();
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<ClienteViewModel>());
            }
        } // fn

        [HttpGet]
        public async Task<IActionResult> EditarCliente(Guid id)
        {
            try
            {
                var cliente = await _clienteApiService.ObtenerAsync(id);

                if (cliente == null)
                {
                    ViewBag.MensajeError = "No se encontró la persona cliente especificada.";
                    return View("ListarClientes", new List<ClienteViewModel>());
                }

                await CargarListasDesplegables();
                return View(cliente);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarClientes", new List<ClienteViewModel>());
            }
        } // fn

        [HttpPost]
        public async Task<IActionResult> EditarCliente(Guid id, ClienteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteApiService.EditarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarClientes");
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
                return View("ListarClientes", new List<ClienteViewModel>());
            }
        } // fn

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(Guid id)
        {
            try
            {
                await _clienteApiService.EliminarAsync(id);
                TempData["MensajeExito"] = "Cliente eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ListarClientes");
        } // fn

        #region MetodosInternos

        private async Task CargarListasDesplegables()
        {
            var tiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionClienteAsync();
            ViewBag.ListaTiposIdentificacion = new SelectList(tiposIdentificacion, "Key", "Value");
        }

        #endregion
    } // fin class
}//fn space
