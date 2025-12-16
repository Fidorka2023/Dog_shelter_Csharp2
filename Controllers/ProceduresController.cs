using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    [Authorize]
    public class ProceduresController : Controller
    {
        private readonly DogShelterDbContext _context;

        public ProceduresController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Procedures
        public async Task<IActionResult> Index()
        {
            var procedures = await _context.Procedures
                .Include(p => p.Record)
                .ToListAsync();
            return View(procedures);
        }

        // GET: Procedures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures
                .Include(p => p.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        // GET: Procedures/Create
        public IActionResult Create()
        {
            ViewData["ZdrZaznamid"] = new SelectList(_context.MedicalRecords, "Id", "Id");
            return View();
        }

        // POST: Procedures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcName,DescrName,ZdrZaznamid")] Procedure procedure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(procedure);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Procedures", "CREATE", null, procedure);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ZdrZaznamid"] = new SelectList(_context.MedicalRecords, "Id", "Id", procedure.ZdrZaznamid);
            return View(procedure);
        }

        // GET: Procedures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure == null)
            {
                return NotFound();
            }
            ViewData["ZdrZaznamid"] = new SelectList(_context.MedicalRecords, "Id", "Id", procedure.ZdrZaznamid);
            return View(procedure);
        }

        // POST: Procedures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProcName,DescrName,ZdrZaznamid")] Procedure procedure)
        {
            if (id != procedure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldProcedure = await _context.Procedures.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    _context.Update(procedure);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Procedures", "UPDATE", oldProcedure, procedure);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcedureExists(procedure.Id))
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
            ViewData["ZdrZaznamid"] = new SelectList(_context.MedicalRecords, "Id", "Id", procedure.ZdrZaznamid);
            return View(procedure);
        }

        // GET: Procedures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedure = await _context.Procedures
                .Include(p => p.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        // POST: Procedures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Procedures", "DELETE", procedure, null);
                _context.Procedures.Remove(procedure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedures.Any(e => e.Id == id);
        }
    }
}

