using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Formulario_soporte.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthFilter(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var nombre = session.GetString("nombre");
            var rol = session.GetString("rol");

            // Si no hay sesión, redirige al login
            if (string.IsNullOrEmpty(nombre))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Si hay roles definidos y el rol del usuario no está entre ellos
            if (_roles != null && _roles.Length > 0 && !_roles.Contains(rol))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
