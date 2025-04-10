﻿﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using AddressBookApp.API.Models;

namespace AddressBookApp.API.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        [HttpGet("/Home")]
        [HttpGet("/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Home/Contacts")]
        [Authorize]
        public IActionResult Contacts()
        {
            return View();
        }

        [HttpGet("/Home/ContactDetails/{id}")]
        [Authorize]
        public IActionResult ContactDetails(Guid id)
        {
            ViewBag.ContactId = id;
            return View();
        }

        [HttpGet("/Home/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
