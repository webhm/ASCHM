using ASCHM.Models;
using ASCHM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace ASCHM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Constructor
        private readonly ILogger<HomeController> _logger;
        IOperation _operation;

        public HomeController(ILogger<HomeController> logger, IOperation operation)
        {
            _logger = logger;
            _operation = operation;
        }
        #endregion

        #region Solicitud  
        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;
            string currentUser = GetCurrentUser(userEmail);           
            var datos = _operation.GetSolicitudesPendientes;
            var filtro = datos.FindAll(x => x.UsuarioAutorizador.Equals(currentUser));
            var autorizador = _operation.GetAutorizador;
            if (currentUser is not null)
            {
                foreach (var access in autorizador)
                {
                    if (access.AutorizadorU.Equals(currentUser))
                    {
                        var opciones = _operation.GetMotivo;
                        opciones.Sort((x, y) => string.Compare(x.Opcion, y.Opcion));
                        List<SelectListItem> items = new List<SelectListItem>();
                        foreach (var item in opciones)
                        {
                            items.Add(new SelectListItem() { Value = item.Id, Text = item.Opcion });
                        }
                        ViewBag.OpcionesJSON = JsonConvert.SerializeObject(items);
                        if (currentUser == "IVIVANCO")
                        {
                            return View(datos);
                        }
                        if (filtro.Count == 0)
                        {
                            TempData["AlertError"] = "No tiene solicitudes pendientes.";

                        }


                        return View(filtro);
                    }
                   
                }
                TempData["AlertError"] = "No existen datos.";
                return View();
            }
            TempData["AlertError"] = "No tiene acceso a este sistema.";
            return View();

        }
       
        public IActionResult Autorizar(int id, int nivel, string usuario)
        {            
            var userEmail = User.Identity.Name;
            string currentUser = GetCurrentUser(userEmail);
            if (currentUser == usuario)
            {
                _operation.Autorizar(currentUser, id, nivel);
            }

            return Json(new { success = true, message = "Elemento autorizado con éxito." });                      
        }

        public IActionResult AutorizarCompletado()
        {
            try
            {
                TempData["AlertAutorizar"] = "Se autorizó correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPersonalizado");
            }
        }


        public IActionResult Rechazar(int id, string opcion, string motivo, int nivel, string usuario)
        {
            int motivoValor = int.Parse(opcion);
            string observacion = motivo;
            var userEmail = User.Identity.Name;
            string currentUser = GetCurrentUser(userEmail);
            if (currentUser == usuario)
            {
                _operation.Cancelar(currentUser, id, nivel, motivoValor, observacion.ToUpper());
            }
            
            return Json(new { success = true, message = "Elemento rechazado con éxito." });
        }


        public IActionResult RechazarCompletado()
        {
            try
            {
                TempData["AlertRechazar"] = "Se rechazó correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPersonalizado");
            }

        }


        public IActionResult ErrorPersonalizado()
        {

            TempData["AlertError"] = "No se ha logrado completar la operación, intente nuevamente.";
            return RedirectToAction("Index");
        }

        public string GetCurrentUser(string email)
        {
            string user = "";
            int indexOfAt = email.IndexOf('@');
            
            if (indexOfAt >= 0)
            {
                user = email.Substring(0, indexOfAt);
            }
            return user.ToUpper();
        }
        #endregion

        #region Historial
       
        public ActionResult VerHis()
        {
            var historial = _operation.GetHistorial;
            var userEmail = User.Identity.Name;
            string currentUser = GetCurrentUser(userEmail);
            var filtro = historial.FindAll(x => x.UsuarioAutorizador.Equals(currentUser));
            if (currentUser == "IVIVANCO")
            {
                return View(historial);
            }
            if (filtro.Count == 0)
            {
                TempData["AlertError"] = "No tiene información.";
            }
            return View(filtro);
        }
        
        [HttpGet]
        public IActionResult GetRegistros(int id)
        {
            var data = _operation.GetNivelAuth;
            List<NivelAuth> filtro = new List<NivelAuth>();
            foreach (var item in data)
            {
                if (id.ToString() == (item.Id))
                {
                    filtro.Add(item);
                }
            }
            return Json(filtro);
        }
        #endregion

        #region Privacy - Error
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

    }
}