using System;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class ApiClientsMvcController : Controller
    {
        private readonly IApiClientService _apiClientService;
        private readonly IMapper _mapper;
        
        public ApiClientsMvcController(IApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var apiClients = await _apiClientService.GetAllApiClientsAsync();
                return View(_mapper.Map<IEnumerable<ApiClientDto>>(apiClients));
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in ApiClientsMvcController.Index: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Return a view with the error message
                ViewBag.ErrorMessage = "An error occurred while loading API clients. Please try again later.";
                return View(new List<ApiClientDto>());
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApiClientCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                var apiClient = await _apiClientService.CreateApiClientAsync(model.Name);
                
                TempData["ApiKey"] = apiClient.ApiKey;
                TempData["ApiSecret"] = apiClient.ApiSecret;
                
                return RedirectToAction(nameof(Credentials), new { id = apiClient.Id });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in ApiClientsMvcController.Create: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
        
        [HttpGet]
        public IActionResult Credentials(Guid id)
        {
            var credentials = new ApiClientCredentialsDto
            {
                ApiKey = TempData["ApiKey"]?.ToString(),
                ApiSecret = TempData["ApiSecret"]?.ToString()
            };
            
            if (string.IsNullOrEmpty(credentials.ApiKey) || string.IsNullOrEmpty(credentials.ApiSecret))
            {
                return RedirectToAction(nameof(Index));
            }
            
            return View(credentials);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _apiClientService.DeleteApiClientAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in ApiClientsMvcController.Delete: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = $"An error occurred while deleting the API client: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
