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
    public class MedicalRecordsController : Controller
    {
        private readonly DogShelterDbContext _context;

        public MedicalRecordsController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: MedicalRecords
        public async Task<IActionResult> Index()
        {
            var medicalRecords = await _context.MedicalRecords
                .Include(m => m.Dog)
                .Include(m => m.Type)
                .ToListAsync();
            return View(medicalRecords);
        }

        // GET: MedicalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Dog)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        public IActionResult Create()
        {
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name");
            ViewData["TypeProcId"] = new SelectList(_context.Procedures, "Id", "ProcName");
            return View();
        }

        // POST: MedicalRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateRec,DogId,TypeProcId")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalRecord);
                await _context.SaveChangesAsync();
                await LogHelper.LogAsync(_context, HttpContext, "MedicalRecords", "CREATE", null, medicalRecord);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", medicalRecord.DogId);
            ViewData["TypeProcId"] = new SelectList(_context.Procedures, "Id", "ProcName", medicalRecord.TypeProcId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", medicalRecord.DogId);
            ViewData["TypeProcId"] = new SelectList(_context.Procedures, "Id", "ProcName", medicalRecord.TypeProcId);
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateRec,DogId,TypeProcId")] MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldMedicalRecord = await _context.MedicalRecords.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "MedicalRecords", "UPDATE", oldMedicalRecord, medicalRecord);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(medicalRecord.Id))
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
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", medicalRecord.DogId);
            ViewData["TypeProcId"] = new SelectList(_context.Procedures, "Id", "ProcName", medicalRecord.TypeProcId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Dog)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "MedicalRecords", "DELETE", medicalRecord, null);
                _context.MedicalRecords.Remove(medicalRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.Id == id);
        }
    }
}

