using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    public class MszeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MszeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Widok dla parafianina (przeglądanie mszy)
        public async Task<IActionResult> Harmonogram(string sortOrder)
        {
            ViewBag.DateSortParm = sortOrder == "desc" ? "asc" : "desc";

            var msze = _context.Msza
                .Include(m => m.Ksiadz)
                .AsQueryable();

            // Sortowanie według parametru
            switch (sortOrder)
            {
                case "desc":
                    msze = msze.OrderByDescending(m => m.Data);
                    break;
                default:
                    msze = msze.OrderBy(m => m.Data);
                    break;
            }

            return View(await msze.ToListAsync());
        }


        // Widok dla księdza (zarządzanie mszami)
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> ZarzadzajMsze()
        {
            var msze = await _context.Msza
                .Include(m => m.Ksiadz) // Pobranie przypisanego księdza
                .OrderBy(m => m.Data)
                .ThenBy(m => m.Godzina)
                .ToListAsync();
            return View(msze);
        }

        // GET: Msze/DodajMsze
        [Authorize(Roles = "Ksiadz")]
        public IActionResult DodajMsze()
        {
            ViewBag.Ksieza = _context.Ksiadz.ToList();
            return View(new Msza());
        }

        // POST: Msze/DodajMsze
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> DodajMsze(Msza msza)
        {
            if (ModelState.IsValid)
            {
                msza.Data = DateTime.SpecifyKind(msza.Data, DateTimeKind.Utc); // Konwersja do UTC
                _context.Msza.Add(msza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ZarzadzajMsze));
            }
            ViewBag.Ksieza = _context.Ksiadz.ToList();
            return View(msza);
        }

        // GET: Msze/EdytujMsze/{id}
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> EdytujMsze(int? id)
        {
            if (id == null) return NotFound();

            var msza = await _context.Msza.FindAsync(id);
            if (msza == null) return NotFound();

            ViewBag.Ksieza = _context.Ksiadz.ToList();
            return View(msza);
        }

        // POST: Msze/EdytujMsze/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ksiadz")]
        public async Task<IActionResult> EdytujMsze(int id, Msza msza)
        {
            if (id != msza.IdMsza) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    msza.Data = DateTime.SpecifyKind(msza.Data, DateTimeKind.Utc); // Konwersja do UTC
                    _context.Update(msza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Msza.Any(m => m.IdMsza == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ZarzadzajMsze));
            }
            ViewBag.Ksieza = _context.Ksiadz.ToList();
            return View(msza);
        }
    }
}
