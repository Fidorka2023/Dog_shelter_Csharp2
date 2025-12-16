using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly DogShelterDbContext _context;

        public AddressesController(DogShelterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Addresses.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();
            return View(address);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,City,Psc,Number")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Addresses", "CREATE", null, address);
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,City,Psc,Number")] Address address)
        {
            if (id != address.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Addresses", "UPDATE", oldAddress, address);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();
            return View(address);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Addresses", "DELETE", address, null);
                _context.Addresses.Remove(address);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id) => _context.Addresses.Any(e => e.Id == id);
    }
}

