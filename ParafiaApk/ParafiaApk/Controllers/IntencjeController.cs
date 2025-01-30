using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    public class IntencjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IntencjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Widok dla parafianina: przeglądanie swoich intencji
       
        [Authorize(Roles = "Parafianin")]
        public async Task<IActionResult> MojeIntencje()
        {
            var userEmail = User.Identity.Name;
            var parafianin = await _context.Parafianin.FirstOrDefaultAsync(p => p.Email == userEmail);

            if (parafianin == null)
                return NotFound("Nie znaleziono użytkownika w bazie danych.");

            var intencje = await _context.Intencja
                .Where(i => i.IdParafianin == parafianin.IdParafianin)
                .OrderBy(i => i.DataZgloszenia)
                .ToListAsync();

            var msze = await _context.Msza.ToListAsync();

            ViewBag.Msze = msze.ToDictionary(m => m.IdMsza);
            return View(intencje);
        }



        // Widok dla parafianina: zgłaszanie intencji
        [Authorize(Roles = "Parafianin")]
        public IActionResult ZglosIntencje()
        {
            ViewBag.Msze = _context.Msza.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parafianin")]
        public async Task<IActionResult> ZglosIntencje(Intencja intencja)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var parafianin = await _context.Parafianin.FirstOrDefaultAsync(p => p.Email == userEmail);

                if (parafianin == null)
                    return NotFound("Nie znaleziono użytkownika w bazie danych.");

                intencja.IdParafianin = parafianin.IdParafianin;
                intencja.Status = "Oczekująca";
                intencja.DataZgloszenia = DateTime.UtcNow;

                _context.Intencja.Add(intencja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojeIntencje));
            }

            ViewBag.Msze = _context.Msza.ToList();
            return View(intencja);
        }

        // Widok dla księdza: zarządzanie intencjami
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> ZarzadzajIntencjami()
        {
            var intencje = await _context.Intencja
                .OrderBy(i => i.DataZgloszenia)
                .ToListAsync();

            var parafianie = await _context.Parafianin.ToDictionaryAsync(p => p.IdParafianin);
            var msze = await _context.Msza.ToDictionaryAsync(m => m.IdMsza);

            ViewBag.Parafianie = parafianie;
            ViewBag.Msze = msze;

            return View(intencje);
        }

        // POST: Zatwierdzanie intencji
        [HttpPost]
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> ZatwierdzIntencje(int id)
        {
            var intencja = await _context.Intencja.FirstOrDefaultAsync(i => i.IdIntencja == id);

            if (intencja == null)
                return NotFound();

            intencja.Status = "Zatwierdzona";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ZarzadzajIntencjami));
        }


    }


}
