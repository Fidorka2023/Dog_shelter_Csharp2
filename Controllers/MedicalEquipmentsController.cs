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
    public class MedicalEquipmentsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public MedicalEquipmentsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: MedicalEquipments
        public async Task<IActionResult> Index()
        {
            var medicalEquipments = await _context.MedicalEquipments
                .Include(m => m.Sklad)
                .ToListAsync();
            return View(medicalEquipments);
        }

        // GET: MedicalEquipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEquipment = await _context.MedicalEquipments
                .Include(m => m.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (medicalEquipment == null)
            {
                return NotFound();
            }

            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Create
        public IActionResult Create()
        {
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name");
            return View();
        }

        // POST: MedicalEquipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MedicalName,Count,SkladID")] MedicalEquipment medicalEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalEquipment);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "MedicalEquipments", "CREATE", null, medicalEquipment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", medicalEquipment.SkladID);
            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEquipment = await _context.MedicalEquipments.FindAsync(id);
            if (medicalEquipment == null)
            {
                return NotFound();
            }
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", medicalEquipment.SkladID);
            return View(medicalEquipment);
        }

        // POST: MedicalEquipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MedicalName,Count,SkladID")] MedicalEquipment medicalEquipment)
        {
            if (id != medicalEquipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldMedicalEquipment = await _context.MedicalEquipments.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    _context.Update(medicalEquipment);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "MedicalEquipments", "UPDATE", oldMedicalEquipment, medicalEquipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalEquipmentExists(medicalEquipment.Id))
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
            ViewData["SkladID"] = new SelectList(_context.Storages, "Id", "Name", medicalEquipment.SkladID);
            return View(medicalEquipment);
        }

        // GET: MedicalEquipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalEquipment = await _context.MedicalEquipments
                .Include(m => m.Sklad)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (medicalEquipment == null)
            {
                return NotFound();
            }

            return View(medicalEquipment);
        }

        // POST: MedicalEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalEquipment = await _context.MedicalEquipments.FindAsync(id);
            if (medicalEquipment != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "MedicalEquipments", "DELETE", medicalEquipment, null);
                _context.MedicalEquipments.Remove(medicalEquipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalEquipmentExists(int id)
        {
            return _context.MedicalEquipments.Any(e => e.Id == id);
        }
    }
}

