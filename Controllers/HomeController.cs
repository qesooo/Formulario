using Formulario_soporte.Filters;
using Formulario_soporte.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Formulario_soporte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AuthFilter("Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AuthFilter("Administrador")]
        public IActionResult AdminMenu()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("nombre");
            return View();
        }

        public IActionResult TecnicoMenu()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("nombre");
            return View();
        }

        public IActionResult SolicitanteMenu()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("nombre");
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
