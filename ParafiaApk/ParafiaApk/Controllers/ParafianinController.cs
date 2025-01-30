using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParafiaApk.Models;

namespace ParafiaApk.Controllers
{
    public class ParafianinController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ParafianinController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user); // Domyślnie ładuje Views/Parafianin/Index.cshtml
        }
    }
}
