using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    [Authorize(requiredPerms: 1)]
    public class LogsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public LogsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Logs.OrderByDescending(l => l.EventTime).ToListAsync());
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }
    }
}

