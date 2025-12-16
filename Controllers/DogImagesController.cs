using DogShelterMvc.Attributes;
using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace DogShelterMvc.Controllers
{
    [Authorize]
    public class DogImagesController : Controller
    {
        private readonly DogShelterDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DogImagesController(DogShelterDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: DogImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.DogImages.ToListAsync());
        }

        // GET: DogImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogImage = await _context.DogImages
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dogImage == null)
            {
                return NotFound();
            }

            return View(dogImage);
        }

        // GET: DogImages/Image/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Image(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogImage = await _context.DogImages.FindAsync(id);
            if (dogImage == null || dogImage.ImageData == null)
            {
                return NotFound();
            }

            // Detekce MIME typu podle přípony souboru
            string contentType = "image/jpeg"; // výchozí
            var extension = Path.GetExtension(dogImage.FileName).ToLowerInvariant();
            switch (extension)
            {
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".jpg":
                case ".jpeg":
                default:
                    contentType = "image/jpeg";
                    break;
            }

            return File(dogImage.ImageData, contentType);
        }

        // GET: DogImages/Create
        public IActionResult Create(int? dogId = null)
        {
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogId);
            return View();
        }

        // POST: DogImages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, string fileName, int? dogId)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Vyberte soubor obrázku");
                ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogId);
                return View();
            }

            // Kontrola typu souboru
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("file", "Povolené formáty: JPG, JPEG, PNG, GIF, BMP");
                ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogId);
                return View();
            }

            // Kontrola velikosti souboru (max 10MB)
            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "Soubor je příliš velký. Maximální velikost je 10MB");
                ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogId);
                return View();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var dogImage = new DogImage
                    {
                        FileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName,
                        ImageData = memoryStream.ToArray()
                    };
                    _context.Add(dogImage);
                    await _context.SaveChangesAsync();

                    // Pokud byl vybrán pes, přiřadíme mu obrázek
                    if (dogId.HasValue)
                    {
                        var dog = await _context.Dogs.FindAsync(dogId.Value);
                        if (dog != null)
                        {
                            dog.ObrazekId = dogImage.Id;
                            await _context.SaveChangesAsync();
                        }
                    }

                    await LogHelper.LogAsync(_context, HttpContext, "DogImages", "CREATE", null, dogImage);
                    TempData["SuccessMessage"] = dogId.HasValue 
                        ? $"Obrázek byl úspěšně přidán a přiřazen k psovi" 
                        : "Obrázek byl úspěšně přidán";
                    
                    if (dogId.HasValue)
                    {
                        return RedirectToAction("Details", "Dogs", new { id = dogId.Value });
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Chyba při ukládání obrázku: {ex.Message}");
                ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Name", dogId);
                return View();
            }
        }

        // GET: DogImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogImage = await _context.DogImages.FindAsync(id);
            if (dogImage == null)
            {
                return NotFound();
            }
            return View(dogImage);
        }

        // POST: DogImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile file, string fileName)
        {
            var dogImage = await _context.DogImages.FindAsync(id);
            if (dogImage == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        dogImage.FileName = fileName;
                    }

                    if (file != null && file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            dogImage.ImageData = memoryStream.ToArray();
                        }
                    }

                    var oldDogImage = await _context.DogImages.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                    _context.Update(dogImage);
                    await _context.SaveChangesAsync();
                    await LogHelper.LogAsync(_context, HttpContext, "DogImages", "UPDATE", oldDogImage, dogImage);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogImageExists(dogImage.Id))
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
            return View(dogImage);
        }

        // GET: DogImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogImage = await _context.DogImages
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (dogImage == null)
            {
                return NotFound();
            }

            return View(dogImage);
        }

        // POST: DogImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogImage = await _context.DogImages.FindAsync(id);
            if (dogImage != null)
            {
                await LogHelper.LogAsync(_context, HttpContext, "DogImages", "DELETE", dogImage, null);
                _context.DogImages.Remove(dogImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogImageExists(int id)
        {
            return _context.DogImages.Any(e => e.Id == id);
        }
    }
}

