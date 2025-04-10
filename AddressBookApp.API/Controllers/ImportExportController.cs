using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class ImportExportController : Controller
    {
        private readonly IImportExportService _importExportService;
        private readonly IMapper _mapper;
        
        public ImportExportController(IImportExportService importExportService, IMapper mapper)
        {
            _importExportService = importExportService;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> ExportHistory()
        {
            var exports = await _importExportService.GetExportHistoryAsync();
            return View(_mapper.Map<IEnumerable<DataExportDto>>(exports));
        }
        
        [HttpGet]
        public async Task<IActionResult> ImportHistory()
        {
            var imports = await _importExportService.GetImportHistoryAsync();
            return View(_mapper.Map<IEnumerable<ImportHistoryDto>>(imports));
        }
        
        [HttpGet]
        public IActionResult ExportContacts()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ExportContacts(bool clientsOnly)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            byte[] fileContents;
            string fileName;
            
            if (clientsOnly)
            {
                fileContents = await _importExportService.ExportClientsToCsvAsync(userId);
                fileName = $"clients_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            }
            else
            {
                fileContents = await _importExportService.ExportContactsToCsvAsync(userId);
                fileName = $"contacts_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            }
            
            return File(fileContents, "text/csv", fileName);
        }
        
        [HttpGet]
        public IActionResult ImportContacts()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ImportContacts(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return View(new ImportResultDto
                {
                    Success = false,
                    Message = "No file selected"
                });
            }
            
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var result = await _importExportService.ImportContactsFromCsvAsync(stream, userId);
                    
                    return View(new ImportResultDto
                    {
                        Success = true,
                        TotalRecords = result.TotalRecords,
                        SuccessfulRecords = result.SuccessfulRecords,
                        FailedRecords = result.FailedRecords,
                        Message = $"Successfully imported {result.SuccessfulRecords} out of {result.TotalRecords} records"
                    });
                }
            }
            catch (Exception ex)
            {
                return View(new ImportResultDto
                {
                    Success = false,
                    Message = $"Error importing contacts: {ex.Message}"
                });
            }
        }
    }
}
