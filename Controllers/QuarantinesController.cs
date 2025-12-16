using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    [Authorize]
    public class QuarantinesController : Controller
    {
        private readonly DogShelterDbContext _context;

        public QuarantinesController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Quarantines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quarantines.ToListAsync());
        }

        // GET: Quarantines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quarantine = await _context.Quarantines
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (quarantine == null)
            {
                return NotFound();
            }

            return View(quarantine);
        }

        // GET: Quarantines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quarantines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BeginOfDate,EndOfDate")] Quarantine quarantine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quarantine);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Quarantines", "CREATE", null, quarantine);
                return RedirectToAction(nameof(Index));
            }
            return View(quarantine);
        }

        // GET: Quarantines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quarantine = await _context.Quarantines.FindAsync(id);
            if (quarantine == null)
            {
                return NotFound();
            }
            return View(quarantine);
        }

        // POST: Quarantines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BeginOfDate,EndOfDate")] Quarantine quarantine)
        {
            if (id != quarantine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldQuarantine = await _context.Quarantines.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
                    _context.Update(quarantine);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Quarantines", "UPDATE", oldQuarantine, quarantine);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuarantineExists(quarantine.Id))
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
            return View(quarantine);
        }

        // GET: Quarantines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quarantine = await _context.Quarantines
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (quarantine == null)
            {
                return NotFound();
            }

            return View(quarantine);
        }

        // POST: Quarantines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quarantine = await _context.Quarantines.FindAsync(id);
            if (quarantine != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Quarantines", "DELETE", quarantine, null);
                _context.Quarantines.Remove(quarantine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuarantineExists(int id)
        {
            return _context.Quarantines.Any(e => e.Id == id);
        }
    }
}

