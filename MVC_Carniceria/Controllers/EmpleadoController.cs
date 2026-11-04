using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Carniceria.Models;
using MVC_Carniceria.Services;
using System.Text.Json;

namespace MVC_Carniceria.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoApiService _empleadoApiService;
        private readonly CatalogosApiService _catalogosApiService;

        //constru
        public EmpleadoController(EmpleadoApiService empleadoApiService, CatalogosApiService catalogosApiService)
        {
            _empleadoApiService = empleadoApiService;
            _catalogosApiService = catalogosApiService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarEmpleados()
        {
            try
            {
                var empleados = await _empleadoApiService.ListarAsync();

                ViewBag.TiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionEmpleadoAsync();
                ViewBag.AreasTrabajo = await _catalogosApiService.ObtenerAreasTrabajoAsync();
                ViewBag.Puestos = await _catalogosApiService.ObtenerPuestosAsync();
                ViewBag.Turnos = await _catalogosApiService.ObtenerTurnosAsync();

                return View(empleados);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<EmpleadoViewModel>());
            }
        }//fn ListarEmpleados

        [HttpGet]
        public async Task<IActionResult> RegistrarEmpleado()
        {
            try
            {
                await CargarListasDesplegables();
                return View(new EmpleadoViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarEmpleados", new List<EmpleadoViewModel>());
            }
        }//fn RegistrarEmpleado

        [HttpPost]
        public async Task<IActionResult> RegistrarEmpleado(EmpleadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoApiService.RegistrarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarEmpleados");
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
                return View("ListarEmpleados", new List<EmpleadoViewModel>());
            }
        }//fn RegistrarEmpleado

        [HttpGet]
        public IActionResult BuscarEmpleado()
        {
            return View(new List<EmpleadoViewModel>());
        }//fn BuscarEmpleado

        [HttpPost]
        public async Task<IActionResult> BuscarEmpleado(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                ViewBag.MensajeInformativo = "Debe ingresar el número de identificación que desea consultar.";
                return View(new List<EmpleadoViewModel>());
            }

            try
            {
                var empleado = await _empleadoApiService.BuscarPorIdentificacionAsync(identificacion);
                var lista = new List<EmpleadoViewModel>();

                if (empleado == null)
                {
                    ViewBag.MensajeInformativo = "No se encontró ninguna persona empleada con la identificación ingresada.";
                }
                else
                {
                    lista.Add(empleado);
                }

                ViewBag.TiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionEmpleadoAsync();
                ViewBag.AreasTrabajo = await _catalogosApiService.ObtenerAreasTrabajoAsync();
                ViewBag.Puestos = await _catalogosApiService.ObtenerPuestosAsync();
                ViewBag.Turnos = await _catalogosApiService.ObtenerTurnosAsync();

                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(new List<EmpleadoViewModel>());
            }
        }//fn BuscarEmpleado

        [HttpGet]
        public async Task<IActionResult> EditarEmpleado(Guid id)
        {
            try
            {
                var empleado = await _empleadoApiService.ObtenerAsync(id);

                if (empleado == null)
                {
                    ViewBag.MensajeError = "No se encontró la persona empleada especificada.";
                    return View("ListarEmpleados", new List<EmpleadoViewModel>());
                }

                await CargarListasDesplegables();
                return View(empleado);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View("ListarEmpleados", new List<EmpleadoViewModel>());
            }
        }//fn EditarEmpleado

        [HttpPost]
        public async Task<IActionResult> EditarEmpleado(Guid id, EmpleadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoApiService.EditarAsync(viewModel);

                    TempData["MensajeExito"] = "Operación realizada correctamente.";
                    return RedirectToAction("ListarEmpleados");
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
                return View("ListarEmpleados", new List<EmpleadoViewModel>());
            }
        }//fn

        [HttpPost]
        public async Task<IActionResult> EliminarEmpleado(Guid id)
        {
            try
            {
                await _empleadoApiService.EliminarAsync(id);
                TempData["MensajeExito"] = "Empleado eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ListarEmpleados");
        }//fn

        #region MetodosInternos

        private async Task CargarListasDesplegables()
        {
            var tiposIdentificacion = await _catalogosApiService.ObtenerTiposIdentificacionEmpleadoAsync();
            var areasTrabajo = await _catalogosApiService.ObtenerAreasTrabajoAsync();
            var puestos = await _catalogosApiService.ObtenerPuestosAsync();
            var turnos = await _catalogosApiService.ObtenerTurnosAsync();
            var mapeo = await _catalogosApiService.ObtenerMapeoAreaPuestoAsync();

            ViewBag.ListaTiposIdentificacion = new SelectList(tiposIdentificacion, "Key", "Value");
            ViewBag.ListaAreasTrabajo = new SelectList(areasTrabajo, "Key", "Value");
            ViewBag.ListaPuestos = new SelectList(puestos, "Key", "Value");
            ViewBag.ListaTurnos = new SelectList(turnos, "Key", "Value");
            ViewBag.MapeoAreaPuesto = JsonSerializer.Serialize(mapeo);

        }//fn CargarListasDesplegables

        #endregion
    }
}//fn space
