using Formulario_soporte.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario_soporte.Models
{
    [AuthFilter]
    public class ReportesController : Controller
    {
        private readonly FormularioDbContext _context;

        public ReportesController(FormularioDbContext context)
        {
            _context = context;
        }

        // GET: Reportes
        public async Task<IActionResult> Index()
        {
            var formularioDbContext = _context.Reportes.Include(r => r.IdEquipoNavigation).Include(r => r.IdSucursalNavigation).Include(r => r.IdUsuarioResponsableNavigation).Include(r => r.IdUsuarioTecnicoNavigation);
            return View(await formularioDbContext.ToListAsync());
        }

        // GET: Reportes/Details/5
        [AuthFilter("Administrador", "Técnico")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes
                .Include(r => r.IdEquipoNavigation)
                .Include(r => r.IdSucursalNavigation)
                .Include(r => r.IdUsuarioResponsableNavigation)
                .Include(r => r.IdUsuarioTecnicoNavigation)
                .FirstOrDefaultAsync(m => m.IdReporte == id);
            if (reporte == null)
            {
                return NotFound();
            }

            // * INICIO: LÓGICA PARA VISUALIZAR LA FIRMA *
            if (reporte.Firma != null && reporte.Firma.Length > 0)
            {
                // Convertimos el byte[] a Base64 y le agregamos el prefijo para usarlo en la etiqueta <img>
                string base64Image = Convert.ToBase64String(reporte.Firma);
                ViewBag.FirmaSrc = $"data:image/png;base64,{base64Image}";
            }
            else
            {
                ViewBag.FirmaSrc = null;
            }
            // * FIN: LÓGICA PARA VISUALIZAR LA FIRMA *


            return View(reporte);
        }

        // GET: Reportes/Create
        [AuthFilter("Administrador", "Técnico")]
        public IActionResult Create()
        {
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "Activo");
            ViewData["IdSucursal"] = new SelectList(
                _context.Sucursals
                    .Select(s => new
                    {
                        s.IdSucursal,
                        NombreCompleto = s.Nombre + " - " + s.Empresa
                    }),
                "IdSucursal",
                "NombreCompleto"
            );
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre");
            ViewData["IdUsuarioTecnico"] = new SelectList(
                _context.Usuarios.Where(u => u.Rol == "Técnico"), // 👈 solo técnicos
                "IdUsuario",
                "Nombre"
            );
            return View();
        }

        // POST: Reportes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReporte,IdUsuarioTecnico,IdEquipo,IdSucursal,FechaReporte,MantenimientoLogico,MantenimientoFisico,MantenimientoReemplazo,Descripcion,TrabajoRealizado,Estado,IdUsuarioResponsable")] Reporte reporte,
            string SignatureData) // recibe el hidden
        {
            // Guardamos la versión completa (dataURL) por si hay que re-renderizar la vista
            string signatureDataUrl = SignatureData;

            // Procesamos la firma si vino
            if (!string.IsNullOrEmpty(SignatureData))
            {
                const string base64Prefix = "data:image/png;base64,";
                if (SignatureData.StartsWith(base64Prefix))
                    SignatureData = SignatureData.Substring(base64Prefix.Length);

                try
                {
                    reporte.Firma = Convert.FromBase64String(SignatureData);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("SignatureData", "El formato de la firma digital es inválido.");
                }
            }
            else
            {
                // Si la firma es obligatoria:
                ModelState.AddModelError("SignatureData", "La firma digital es requerida.");
                // Si no es obligatoria, comentá o quitá la línea anterior.
            }

            if (ModelState.IsValid)
            {
                _context.Add(reporte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si llegamos acá, ModelState inválido: volver a poblar los selectlists
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "Activo", reporte.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(
                _context.Sucursals.Select(s => new { s.IdSucursal, NombreCompleto = s.Nombre + " - " + s.Empresa }),
                "IdSucursal",
                "NombreCompleto",
                reporte.IdSucursal
            );
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", reporte.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios.Where(u => u.Rol == "Técnico"), "IdUsuario", "Nombre", reporte.IdUsuarioTecnico);

            // Importante: pasamos el dataURL original de vuelta a la vista para que no se pierda
            ViewData["SignatureData"] = signatureDataUrl;

            return View(reporte);
        }

        // GET: Reportes/Edit/5
        [AuthFilter("Administrador", "Técnico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null)
            {
                return NotFound();
            }
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "Activo", reporte.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(
                _context.Sucursals
                    .Select(s => new
                    {
                        s.IdSucursal,
                        NombreCompleto = s.Nombre + " - " + s.Empresa
                    }),
                "IdSucursal",
                "NombreCompleto"
            );
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", reporte.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(
                _context.Usuarios.Where(u => u.Rol == "Técnico"),
                "IdUsuario",
                "Nombre",
                reporte.IdUsuarioTecnico
            );
            return View(reporte);
        }

        // POST: Reportes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id) // 👈 Solo necesitamos el 'id'
        {
            var reporteExistente = await _context.Reportes.FindAsync(id);

            if (reporteExistente == null)
            {
                return NotFound();
            }

            // Usamos TryUpdateModelAsync para aplicar los valores del formulario
            // directamente sobre el objeto que cargamos de la BD.
            // Esto asegura que Entity Framework detecte los cambios.
            // Se listan solo las propiedades que se permite modificar desde el formulario.
            var isUpdateSuccessful = await TryUpdateModelAsync(
                reporteExistente,
                "", // Prefijo vacío, ya que los nombres de los campos del form coinciden con el modelo
                r => r.IdUsuarioTecnico, r => r.IdEquipo, r => r.IdSucursal, r => r.FechaReporte,
                r => r.MantenimientoLogico, r => r.MantenimientoFisico, r => r.MantenimientoReemplazo,
                r => r.Descripcion, r => r.TrabajoRealizado, r => r.Estado, r => r.IdUsuarioResponsable
            );

            // Si la actualización fue exitosa y el modelo es válido...
            if (isUpdateSuccessful && ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync(); // Guardamos los cambios detectados
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reportes.Any(e => e.IdReporte == reporteExistente.IdReporte))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si llegamos aquí es porque hubo un error de validación.
            // Volvemos a poblar los SelectLists para mostrar el formulario de nuevo.
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "Activo", reporteExistente.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(
                _context.Sucursals.Select(s => new { s.IdSucursal, NombreCompleto = s.Nombre + " - " + s.Empresa }),
                "IdSucursal", "NombreCompleto", reporteExistente.IdSucursal);
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", reporteExistente.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios.Where(u => u.Rol == "Técnico"), "IdUsuario", "Nombre", reporteExistente.IdUsuarioTecnico);

            return View(reporteExistente);
        }


        // GET: Reportes/Delete/5
        [AuthFilter("Administrador", "Técnico")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes
                .Include(r => r.IdEquipoNavigation)
                .Include(r => r.IdSucursalNavigation)
                .Include(r => r.IdUsuarioResponsableNavigation)
                .Include(r => r.IdUsuarioTecnicoNavigation)
                .FirstOrDefaultAsync(m => m.IdReporte == id);
            if (reporte == null)
            {
                return NotFound();
            }

            return View(reporte);
        }

        // POST: Reportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte != null)
            {
                _context.Reportes.Remove(reporte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReporteExists(int id)
        {
            return _context.Reportes.Any(e => e.IdReporte == id);
        }
    }
}
