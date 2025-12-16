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
    public class FeedsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public FeedsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Feeds
        public async Task<IActionResult> Index()
        {
            var feeds = await _context.Feeds
                .Include(f => f.Sklad)
                .ToListAsync();
            return View(feeds);
        }

        // GET: Feeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds
                .Include(f => f.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // GET: Feeds/Create
        public IActionResult Create()
        {
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name");
            return View();
        }

        // POST: Feeds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazev,Pocet,SkladID")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feed);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "Feeds", "CREATE", null, feed);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", feed.SkladID);
            return View(feed);
        }

        // GET: Feeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", feed.SkladID);
            return View(feed);
        }

        // POST: Feeds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazev,Pocet,SkladID")] Feed feed)
        {
            if (id != feed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldFeed = await _context.Feeds.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
                    _context.Update(feed);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "Feeds", "UPDATE", oldFeed, feed);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedExists(feed.Id))
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
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", feed.SkladID);
            return View(feed);
        }

        // GET: Feeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds
                .Include(f => f.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "Feeds", "DELETE", feed, null);
                _context.Feeds.Remove(feed);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedExists(int id)
        {
            return _context.Feeds.Any(e => e.Id == id);
        }
    }
}

