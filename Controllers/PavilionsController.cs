using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    [Authorize]
    public class PavilionsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public PavilionsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Pavilions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pavilions.ToListAsync());
        }

        // GET: Pavilions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pavilion = await _context.Pavilions
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (pavilion == null)
            {
                return NotFound();
            }

            return View(pavilion);
        }

        // GET: Pavilions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pavilions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PavName,CapacityPav")] Pavilion pavilion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pavilion);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Pavilions", "CREATE", null, pavilion);
                return RedirectToAction(nameof(Index));
            }
            return View(pavilion);
        }

        // GET: Pavilions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pavilion = await _context.Pavilions.FindAsync(id);
            if (pavilion == null)
            {
                return NotFound();
            }
            return View(pavilion);
        }

        // POST: Pavilions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PavName,CapacityPav")] Pavilion pavilion)
        {
            if (id != pavilion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldPavilion = await _context.Pavilions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    _context.Update(pavilion);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Pavilions", "UPDATE", oldPavilion, pavilion);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PavilionExists(pavilion.Id))
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
            return View(pavilion);
        }

        // GET: Pavilions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pavilion = await _context.Pavilions
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (pavilion == null)
            {
                return NotFound();
            }

            return View(pavilion);
        }

        // POST: Pavilions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pavilion = await _context.Pavilions.FindAsync(id);
            if (pavilion != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Pavilions", "DELETE", pavilion, null);
                _context.Pavilions.Remove(pavilion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PavilionExists(int id)
        {
            return _context.Pavilions.Any(e => e.Id == id);
        }
    }
}

