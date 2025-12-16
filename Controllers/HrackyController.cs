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
    public class HrackyController : Controller
    {
        private readonly DogShelterDbContext _context;

        public HrackyController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Hracky
        public async Task<IActionResult> Index()
        {
            var hracky = await _context.Hracky
                .Include(h => h.Sklad)
                .ToListAsync();
            return View(hracky);
        }

        // GET: Hracky/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hracka = await _context.Hracky
                .Include(h => h.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (hracka == null)
            {
                return NotFound();
            }

            return View(hracka);
        }

        // GET: Hracky/Create
        public IActionResult Create()
        {
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name");
            return View();
        }

        // POST: Hracky/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazev,Pocet,SkladID")] Hracka hracka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hracka);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Hracky", "CREATE", null, hracka);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", hracka.SkladID);
            return View(hracka);
        }

        // GET: Hracky/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hracka = await _context.Hracky.FindAsync(id);
            if (hracka == null)
            {
                return NotFound();
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", hracka.SkladID);
            return View(hracka);
        }

        // POST: Hracky/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazev,Pocet,SkladID")] Hracka hracka)
        {
            if (id != hracka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldHracka = await _context.Hracky.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                    _context.Update(hracka);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Hracky", "UPDATE", oldHracka, hracka);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HrackaExists(hracka.Id))
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
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", hracka.SkladID);
            return View(hracka);
        }

        // GET: Hracky/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hracka = await _context.Hracky
                .Include(h => h.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (hracka == null)
            {
                return NotFound();
            }

            return View(hracka);
        }

        // POST: Hracky/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hracka = await _context.Hracky.FindAsync(id);
            if (hracka != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Hracky", "DELETE", hracka, null);
                _context.Hracky.Remove(hracka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HrackaExists(int id)
        {
            return _context.Hracky.Any(e => e.Id == id);
        }
    }
}

