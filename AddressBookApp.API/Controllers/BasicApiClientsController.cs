using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class BasicApiClientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ViewBag.ErrorMessage = "Name is required";
                return View();
            }
            
            // Generate fake API key and secret
            var apiKey = System.Guid.NewGuid().ToString("N");
            var apiSecret = System.Guid.NewGuid().ToString("N") + System.Guid.NewGuid().ToString("N");
            
            ViewBag.ApiKey = apiKey;
            ViewBag.ApiSecret = apiSecret;
            
            return View("Credentials");
        }
        
        public IActionResult Credentials()
        {
            return View();
        }
    }
}
