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
    public class ApiClientsViewsController : Controller
    {
        private readonly IApiClientService _apiClientService;
        private readonly IMapper _mapper;
        
        public ApiClientsViewsController(IApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            var apiClients = await _apiClientService.GetAllApiClientsAsync();
            return View(_mapper.Map<IEnumerable<ApiClientDto>>(apiClients));
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
            
            var apiClient = await _apiClientService.CreateApiClientAsync(model.Name);
            
            TempData["ApiKey"] = apiClient.ApiKey;
            TempData["ApiSecret"] = apiClient.ApiSecret;
            
            return RedirectToAction(nameof(Credentials), new { id = apiClient.Id });
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
            await _apiClientService.DeleteApiClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
