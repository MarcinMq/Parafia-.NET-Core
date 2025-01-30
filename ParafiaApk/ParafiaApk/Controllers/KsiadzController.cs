using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    public class KsiadzController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public KsiadzController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ksiadz = await _context.Ksiadz
                .FirstOrDefaultAsync(k => k.ApplicationUserId == user.Id);

            if (ksiadz == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(ksiadz);
        }
    }
}
