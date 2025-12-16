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
    public class DogHistoriesController : Controller
    {
        private readonly DogShelterDbContext _context;

        public DogHistoriesController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: DogHistories
        public async Task<IActionResult> Index()
        {
            var dogHistories = await _context.DogHistories
                .Include(d => d.Pes)
                .ToListAsync();
            return View(dogHistories);
        }

        // GET: DogHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogHistory = await _context.DogHistories
                .Include(d => d.Pes)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dogHistory == null)
            {
                return NotFound();
            }

            return View(dogHistory);
        }

        // GET: DogHistories/Create
        public IActionResult Create()
        {
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name");
            return View();
        }

        // POST: DogHistories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfEvent,EventDescription,TypeId,DogId,Typ")] DogHistory dogHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dogHistory);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "DogHistories", "CREATE", null, dogHistory);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogHistory.DogId);
            return View(dogHistory);
        }

        // GET: DogHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogHistory = await _context.DogHistories.FindAsync(id);
            if (dogHistory == null)
            {
                return NotFound();
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogHistory.DogId);
            return View(dogHistory);
        }

        // POST: DogHistories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfEvent,EventDescription,TypeId,DogId,Typ")] DogHistory dogHistory)
        {
            if (id != dogHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldDogHistory = await _context.DogHistories.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                    _context.Update(dogHistory);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "DogHistories", "UPDATE", oldDogHistory, dogHistory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogHistoryExists(dogHistory.Id))
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
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogHistory.DogId);
            return View(dogHistory);
        }

        // GET: DogHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogHistory = await _context.DogHistories
                .Include(d => d.Pes)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dogHistory == null)
            {
                return NotFound();
            }

            return View(dogHistory);
        }

        // POST: DogHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogHistory = await _context.DogHistories.FindAsync(id);
            if (dogHistory != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "DogHistories", "DELETE", dogHistory, null);
                _context.DogHistories.Remove(dogHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogHistoryExists(int id)
        {
            return _context.DogHistories.Any(e => e.Id == id);
        }
    }
}

