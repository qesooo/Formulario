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

            return View(reporte);
        }

        // GET: Reportes/Create
        [AuthFilter("Administrador", "Técnico")]
        public IActionResult Create()
        {
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "IdEquipo");
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "IdSucursal", "IdSucursal");
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Reportes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReporte,IdUsuarioTecnico,IdEquipo,IdSucursal,FechaReporte,TipoMantenimiento,Descripcion,TrabajoRealizado,Estado,IdUsuarioResponsable")] Reporte reporte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reporte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "IdEquipo", reporte.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "IdSucursal", "IdSucursal", reporte.IdSucursal);
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioTecnico);
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
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "IdEquipo", reporte.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "IdSucursal", "IdSucursal", reporte.IdSucursal);
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioTecnico);
            return View(reporte);
        }

        // POST: Reportes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReporte,IdUsuarioTecnico,IdEquipo,IdSucursal,FechaReporte,TipoMantenimiento,Descripcion,TrabajoRealizado,Estado,IdUsuarioResponsable")] Reporte reporte)
        {
            if (id != reporte.IdReporte)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporteExists(reporte.IdReporte))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEquipo"] = new SelectList(_context.Equipos, "IdEquipo", "IdEquipo", reporte.IdEquipo);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "IdSucursal", "IdSucursal", reporte.IdSucursal);
            ViewData["IdUsuarioResponsable"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioResponsable);
            ViewData["IdUsuarioTecnico"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", reporte.IdUsuarioTecnico);
            return View(reporte);
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
