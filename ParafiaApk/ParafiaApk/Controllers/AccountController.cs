﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;
using ParafiaApk.ViewModels;
using System.Threading.Tasks;

namespace ParafiaApk.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Imie = model.Imie,
                    Nazwisko = model.Nazwisko,
                    PhoneNumber = model.PhoneNumber,
                    Parafia = null, // Domyślnie puste
                    Stanowisko = null // Domyślnie puste
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Dodanie roli "Parafianin" dla nowego użytkownika
                    if (!await _userManager.IsInRoleAsync(user, "Parafianin"))
                    {
                        await _userManager.AddToRoleAsync(user, "Parafianin");
                    }

                    // Dodanie parafianina do tabeli Parafianin
                    var Parafianin = new Parafianin
                    {
                        Imie = model.Imie,
                        Nazwisko = model.Nazwisko,
                        Email = model.Email,
                        Telefon = model.PhoneNumber,
                        Parafia = "", // Domyślnie puste
                        Stanowisko = "", // Domyślnie puste
                        ApplicationUserId = user.Id // Powiązanie z ApplicationUser
                    };

                    _context.Parafianin.Add(Parafianin);
                    await _context.SaveChangesAsync();

                    // Logowanie użytkownika po zarejestrowaniu
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Dodanie komunikatów o błędach
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Błędny login lub hasło.");
            }

            return View(model);
        }

        // GET: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
