using Microsoft.AspNetCore.Mvc;
using Formulario_soporte.Models;

namespace Formulario_soporte.Controllers
{
    public class LoginController : Controller
    {
        private readonly FormularioDbContext _context; 

        public LoginController(FormularioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, string password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Nombre == nombre && u.Password == password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("nombre", usuario.Nombre);
                HttpContext.Session.SetString("rol", usuario.Rol);

                // Redirigir según el rol del usuario
                return usuario.Rol switch
                {
                    "Administrador" => RedirectToAction("AdminMenu", "Home"),
                    "Técnico" => RedirectToAction("TecnicoMenu", "Home"),
                    "Solicitante" => RedirectToAction("SolicitanteMenu", "Home"),
                    _ => RedirectToAction("Index")
                };
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
