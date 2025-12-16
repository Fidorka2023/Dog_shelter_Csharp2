using DogShelterMvc.Data;
using DogShelterMvc.Helpers;
using DogShelterMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly DogShelterDbContext _context;

        public AccountController(DogShelterDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Uživatelské jméno a heslo jsou povinné.");
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Uname == username);

            if (user == null || !PasswordHelper.VerifyPassword(password, user.Hash))
            {
                ModelState.AddModelError("", "Nesprávné uživatelské jméno nebo heslo.");
                return View();
            }

            // Uložení informací o uživateli do session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Uname);
            HttpContext.Session.SetString("Perms", user.Perms.ToString());

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Uživatelské jméno a heslo jsou povinné.");
                return View();
            }

            // Kontrola, zda uživatel již existuje
            if (await _context.Users.AnyAsync(u => u.Uname == username))
            {
                ModelState.AddModelError("", "Uživatelské jméno již existuje.");
                return View();
            }

            var user = new User
            {
                Uname = username,
                Hash = PasswordHelper.HashPassword(password),
                Perms = 0  // Výchozí oprávnění - uživatel nemůže nastavit při registraci
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Uživatel byl úspěšně vytvořen.";
            return RedirectToAction("Login");
        }
    }
}

