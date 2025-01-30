using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    public class SakramentyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SakramentyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Widok dla parafianina: przeglądanie zgłoszeń sakramentów
        [Authorize(Roles = "Parafianin")]
        public async Task<IActionResult> MojeZgloszenia()
        {
            var userEmail = User.Identity.Name;
            var parafianin = await _context.Parafianin.FirstOrDefaultAsync(p => p.Email == userEmail);

            if (parafianin == null)
                return NotFound("Nie znaleziono użytkownika w bazie danych.");

            var zgloszenia = await _context.Sakrament
                .Include(s => s.Ksiadz) // Załaduj dane Księdza
                .Where(s => s.IdParafianin == parafianin.IdParafianin)
                .OrderBy(s => s.Data)
                .ToListAsync();

            return View(zgloszenia);
        }


        // 🔹 Widok dla parafianina: zgłaszanie sakramentu
        [Authorize(Roles = "Parafianin")]
        public async Task<IActionResult> ZglosSakrament()
        {
            ViewBag.Ksieza = await _context.Ksiadz.ToListAsync(); // Pobranie listy księży
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parafianin")]
        public async Task<IActionResult> ZglosSakrament(Sakrament sakrament)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var parafianin = await _context.Parafianin.FirstOrDefaultAsync(p => p.Email == userEmail);

                if (parafianin == null)
                    return NotFound("Nie znaleziono użytkownika w bazie danych.");

                sakrament.IdParafianin = parafianin.IdParafianin;
                sakrament.Data = DateTime.UtcNow; // Ustawienie daty w UTC

                // Jeśli użytkownik nie wybrał księdza, ustaw domyślną wartość 0
                if (sakrament.IdKsiadz == 0)
                {
                    sakrament.IdKsiadz = 0;
                }

                _context.Sakrament.Add(sakrament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojeZgloszenia));
            }

            ViewBag.Ksieza = await _context.Ksiadz.ToListAsync(); // Pobranie listy księży na nowo w razie błędu
            return View(sakrament);
        }

        // 🔹 Widok dla księdza: zarządzanie zgłoszeniami sakramentów
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> ZarzadzajZgloszeniami()
        {
            var zgloszenia = await _context.Sakrament
                .Include(s => s.Ksiadz) // Pobranie księdza
                .Include(s => s.Parafianin) // Pobranie parafianina
                .OrderBy(s => s.Data)
                .ToListAsync();

            var parafianie = await _context.Parafianin.ToDictionaryAsync(p => p.IdParafianin, p => $"{p.Imie} {p.Nazwisko}");
            var ksieza = await _context.Ksiadz.ToDictionaryAsync(k => k.IdKsiadz, k => $"{k.Imie} {k.Nazwisko}");

            ViewBag.Parafianie = parafianie;
            ViewBag.Ksieza = ksieza;

            return View(zgloszenia);
        }

        // 🔹 GET: Przypisanie księdza do zgłoszenia
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> PrzypiszKsiadz(int id)
        {
            var sakrament = await _context.Sakrament.FirstOrDefaultAsync(s => s.IdSakrament == id);

            if (sakrament == null)
                return NotFound();

            ViewBag.Ksieza = await _context.Ksiadz.ToListAsync();
            return View(sakrament);
        }

        // 🔹 POST: Przypisanie księdza do zgłoszenia
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> PrzypiszKsiadz(int id, int idKsiadz)
        {
            var sakrament = await _context.Sakrament.FindAsync(id);

            if (sakrament == null)
                return NotFound();

            sakrament.IdKsiadz = idKsiadz;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ZarzadzajZgloszeniami));
        }
        [HttpPost]
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> ZatwierdzSakrament(int id)
        {
            var sakrament = await _context.Sakrament.FindAsync(id);

            if (sakrament == null)
                return NotFound();

            sakrament.Status = "Zatwierdzone"; // Ręczna zmiana statusu przez księdza
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ZarzadzajZgloszeniami));
        }
    }
}
