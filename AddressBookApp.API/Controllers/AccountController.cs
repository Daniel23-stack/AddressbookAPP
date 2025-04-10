﻿﻿﻿﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AddressBookApp.API.Models;
using AddressBookApp.Core.Interfaces;
using System.Threading.Tasks;

namespace AddressBookApp.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, token, user) = await _authService.LoginAsync(model.Email, model.Password);

            if (!success)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("AccessToken", token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToLocal(returnUrl);
        }

        [HttpGet("/Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("/Account/SimpleRegister")]
        public IActionResult SimpleRegister()
        {
            return View();
        }

        [HttpPost("/Account/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Debug information
            Console.WriteLine("Register action called");
            Console.WriteLine($"Email: {model?.Email}");
            Console.WriteLine($"FirstName: {model?.FirstName}");
            Console.WriteLine($"LastName: {model?.LastName}");
            Console.WriteLine($"Password length: {model?.Password?.Length ?? 0}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                Console.WriteLine("Passwords do not match");
                ModelState.AddModelError("", "Passwords do not match");
                return View(model);
            }

            try
            {
                Console.WriteLine("Calling AuthService.RegisterAsync");
                var (success, message) = await _authService.RegisterAsync(
                    model.Email,
                    model.Password,
                    model.FirstName,
                    model.LastName);

                Console.WriteLine($"Registration result: {success}, Message: {message}");

                if (!success)
                {
                    ModelState.AddModelError("", message);
                    return View(model);
                }

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Contacts), "Home");
            }
        }
    }
}
