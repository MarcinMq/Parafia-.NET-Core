using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    [Authorize(Roles = "Ksiadz")] // Tylko księża mogą dodawać ogłoszenia
    public class OgloszeniaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OgloszeniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ogloszenia/Lista
        [AllowAnonymous] // Każdy użytkownik może przeglądać listę ogłoszeń
        public async Task<IActionResult> Lista(string sortOrder)
        {
            ViewBag.SortParam = sortOrder == "desc" ? "asc" : "desc"; // Przełącznik sortowania

            var ogloszenia = _context.Ogloszenie.AsQueryable();

            if (sortOrder == "asc")
            {
                ogloszenia = ogloszenia.OrderBy(o => o.DataUtworzenia);
            }
            else
            {
                ogloszenia = ogloszenia.OrderByDescending(o => o.DataUtworzenia);
            }

            return View(await ogloszenia.ToListAsync());
        }

        // GET: Ogloszenia/Dodaj
        public IActionResult Dodaj()
        {
            return View();
        }

        // POST: Ogloszenia/Dodaj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                // Pobierz ID zalogowanego użytkownika
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Znajdź księdza na podstawie ApplicationUserId
                var ksiadz = await _context.Ksiadz
                    .FirstOrDefaultAsync(k => k.ApplicationUserId == userId);

                if (ksiadz == null)
                {
                    ModelState.AddModelError("", "Nie znaleziono przypisanego księdza.");
                    return View(ogloszenie);
                }

                ogloszenie.IdKsiadz = ksiadz.IdKsiadz; // Przypisz ID księdza
                ogloszenie.DataUtworzenia = DateTime.UtcNow; // Ustaw datę dodania
                _context.Ogloszenie.Add(ogloszenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }

            return View(ogloszenie);
        }
    }
}
