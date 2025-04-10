using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class SimpleApiClientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            return View();
        }
    }
}
