using System;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    public class BasicRegisterController : Controller
    {
        private readonly IAuthService _authService;

        public BasicRegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string firstName, string lastName, string password, string confirmPassword)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || 
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password) || 
                string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.ErrorMessage = "All fields are required";
                return View("Index");
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match";
                return View("Index");
            }

            try
            {
                var (success, message) = await _authService.RegisterAsync(
                    email,
                    password,
                    firstName,
                    lastName);

                if (!success)
                {
                    ViewBag.ErrorMessage = message;
                    return View("Index");
                }

                ViewBag.SuccessMessage = "Registration successful! You can now log in.";
                return View("Success");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Index");
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
