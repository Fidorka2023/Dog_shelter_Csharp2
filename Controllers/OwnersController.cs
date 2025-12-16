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
    public class OwnersController : Controller
    {
        private readonly DogShelterDbContext _context;

        public OwnersController(DogShelterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var owners = await _context.Owners.Include(o => o.Adresa).ToListAsync();
            return View(owners);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var owner = await _context.Owners.Include(o => o.Adresa).FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null) return NotFound();
            return View(owner);
        }

        public IActionResult Create()
        {
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Phone,Email,AddressID")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(owner);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Owners", "CREATE", null, owner);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", owner.AddressID);
            return View(owner);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null) return NotFound();
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", owner.AddressID);
            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Phone,Email,AddressID")] Owner owner)
        {
            if (id != owner.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldOwner = await _context.Owners.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Owners", "UPDATE", oldOwner, owner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressID"] = new SelectList(_context.Addresses, "Id", "Street", owner.AddressID);
            return View(owner);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var owner = await _context.Owners.Include(o => o.Adresa).FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null) return NotFound();
            return View(owner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Owners", "DELETE", owner, null);
                _context.Owners.Remove(owner);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id) => _context.Owners.Any(e => e.Id == id);
    }
}

