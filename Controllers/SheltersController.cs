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
    public class SheltersController : Controller
    {
        private readonly DogShelterDbContext _context;

        public SheltersController(DogShelterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shelters = await _context.Shelters.Include(s => s.Adresa).ToListAsync();
            return View(shelters);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var shelter = await _context.Shelters.Include(s => s.Adresa).FirstOrDefaultAsync(m => m.Id == id);
            if (shelter == null) return NotFound();
            return View(shelter);
        }

        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Telephone,Email,AddressID")] Shelter shelter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shelter);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Shelters", "CREATE", null, shelter);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", shelter.AddressID);
            return View(shelter);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null) return NotFound();
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", shelter.AddressID);
            return View(shelter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Telephone,Email,AddressID")] Shelter shelter)
        {
            if (id != shelter.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldShelter = await _context.Shelters.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                    _context.Update(shelter);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Shelters", "UPDATE", oldShelter, shelter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelterExists(shelter.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", shelter.AddressID);
            return View(shelter);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var shelter = await _context.Shelters.Include(s => s.Adresa).FirstOrDefaultAsync(m => m.Id == id);
            if (shelter == null) return NotFound();
            return View(shelter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Shelters", "DELETE", shelter, null);
                _context.Shelters.Remove(shelter);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelterExists(int id) => _context.Shelters.Any(e => e.Id == id);
    }
}

