using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    public class DogsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public DogsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Dogs
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var dogs = await _context.Dogs
                .Include(d => d.Utulek)
                .Include(d => d.Majitel)
                .Include(d => d.Karantena)
                .Include(d => d.DogImage)
                .ToListAsync();
            return View(dogs);
        }

        // GET: Dogs/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Utulek)
                .Include(d => d.Majitel)
                .Include(d => d.Karantena)
                .Include(d => d.Otec)
                .Include(d => d.Matka)
                .Include(d => d.DogImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Dogs/Create
        [Attributes.Authorize]
        public IActionResult Create()
        {
            if (!PermissionHelper.CanEdit(HttpContext))
            {
                TempData["ErrorMessage"] = "Nemáte oprávnění k vytváření nových záznamů.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["UtulekId"] = new SelectList(_context.Shelters, "Id", "Name");
            ViewData["MajitelId"] = new SelectList(_context.Owners, "Id", "Name");
            ViewData["KarantenaId"] = new SelectList(_context.Quarantines, "Id", "Id");
            ViewData["OtecId"] = new SelectList(_context.Dogs, "Id", "Name");
            ViewData["MatkaId"] = new SelectList(_context.Dogs, "Id", "Name");
            ViewData["ObrazekId"] = new SelectList(_context.DogImages, "Id", "FileName");
            return View();
        }

        // POST: Dogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Attributes.Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,BodyColor,DatumPrijeti,DuvodPrijeti,StavPes,UtulekId,KarantenaId,MajitelId,OtecId,MatkaId,ObrazekId")] Dog dog)
        {
            if (!PermissionHelper.CanEdit(HttpContext))
            {
                TempData["ErrorMessage"] = "Nemáte oprávnění k vytváření nových záznamů.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Add(dog);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Dogs", "CREATE", null, dog);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtulekId"] = new SelectList(_context.Shelters, "Id", "Name", dog.UtulekId);
            ViewData["MajitelId"] = new SelectList(_context.Owners, "Id", "Name", dog.MajitelId);
            ViewData["KarantenaId"] = new SelectList(_context.Quarantines, "Id", "Id", dog.KarantenaId);
            ViewData["OtecId"] = new SelectList(_context.Dogs, "Id", "Name", dog.OtecId);
            ViewData["MatkaId"] = new SelectList(_context.Dogs, "Id", "Name", dog.MatkaId);
            ViewData["ObrazekId"] = new SelectList(_context.DogImages, "Id", "FileName", dog.ObrazekId);
            return View(dog);
        }

        // GET: Dogs/Edit/5
        [Attributes.Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog == null)
            {
                return NotFound();
            }
            ViewData["UtulekId"] = new SelectList(_context.Shelters, "Id", "Name", dog.UtulekId);
            ViewData["MajitelId"] = new SelectList(_context.Owners, "Id", "Name", dog.MajitelId);
            ViewData["KarantenaId"] = new SelectList(_context.Quarantines, "Id", "Id", dog.KarantenaId);
            ViewData["OtecId"] = new SelectList(_context.Dogs, "Id", "Name", dog.OtecId);
            ViewData["MatkaId"] = new SelectList(_context.Dogs, "Id", "Name", dog.MatkaId);
            ViewData["ObrazekId"] = new SelectList(_context.DogImages, "Id", "FileName", dog.ObrazekId);
            return View(dog);
        }

        // POST: Dogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Attributes.Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,BodyColor,DatumPrijeti,DuvodPrijeti,StavPes,UtulekId,KarantenaId,MajitelId,OtecId,MatkaId,ObrazekId")] Dog dog)
        {
            if (id != dog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldDog = await _context.Dogs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Dogs", "UPDATE", oldDog, dog);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.Id))
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
            ViewData["UtulekId"] = new SelectList(_context.Shelters, "Id", "Name", dog.UtulekId);
            ViewData["MajitelId"] = new SelectList(_context.Owners, "Id", "Name", dog.MajitelId);
            ViewData["KarantenaId"] = new SelectList(_context.Quarantines, "Id", "Id", dog.KarantenaId);
            ViewData["OtecId"] = new SelectList(_context.Dogs, "Id", "Name", dog.OtecId);
            ViewData["MatkaId"] = new SelectList(_context.Dogs, "Id", "Name", dog.MatkaId);
            ViewData["ObrazekId"] = new SelectList(_context.DogImages, "Id", "FileName", dog.ObrazekId);
            return View(dog);
        }

        // GET: Dogs/Delete/5
        [Attributes.Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Utulek)
                .Include(d => d.Majitel)
                .Include(d => d.Karantena)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Attributes.Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!PermissionHelper.CanDelete(HttpContext))
            {
                TempData["ErrorMessage"] = "Nemáte oprávnění k mazání záznamů.";
                return RedirectToAction(nameof(Index));
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Dogs", "DELETE", dog, null);
                _context.Dogs.Remove(dog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.Id == id);
        }
    }
}

