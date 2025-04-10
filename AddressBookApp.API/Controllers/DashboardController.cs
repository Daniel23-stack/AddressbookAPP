using System.Security.Claims;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardDto
            {
                TotalContacts = await _dashboardService.GetTotalContactsCountAsync(),
                ClientContacts = await _dashboardService.GetClientContactsCountAsync(),
                ImportedContacts = await _dashboardService.GetImportedContactsCountAsync(),
                DataExportCount = await _dashboardService.GetDataExportCountAsync(),
                ApiClientUsageCount = await _dashboardService.GetApiClientUsageCountLastDaysAsync(7)
            };
            
            ViewBag.UserName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            
            return View(dashboard);
        }
    }
}
