using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AddressBookApp.API.Controllers
{
    [Route("api/external/contacts")]
    [ApiController]
    public class ContactsApiController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IContactRepository _contactRepository;
        private readonly IApiClientService _apiClientService;
        private readonly IMapper _mapper;
        
        public ContactsApiController(
            IContactService contactService,
            IContactRepository contactRepository,
            IApiClientService apiClientService,
            IMapper mapper)
        {
            _contactService = contactService;
            _contactRepository = contactRepository;
            _apiClientService = apiClientService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Validate API credentials
            if (!Request.Headers.TryGetValue("X-Api-Key", out StringValues apiKey) ||
                !Request.Headers.TryGetValue("X-Api-Secret", out StringValues apiSecret))
            {
                return Unauthorized(new { Message = "API credentials are required" });
            }
            
            bool isValid = await _apiClientService.ValidateApiCredentialsAsync(apiKey, apiSecret);
            if (!isValid)
            {
                return Unauthorized(new { Message = "Invalid API credentials" });
            }
            
            // Get API client
            var apiClient = await _apiClientService.GetApiClientByApiKeyAsync(apiKey);
            
            // Log API usage
            await _apiClientService.LogApiUsageAsync(
                apiClient.Id,
                "GET /api/external/contacts",
                true,
                $"startDate={startDate}, endDate={endDate}",
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
            );
            
            // Get contacts by date range if specified
            IReadOnlyList<Core.Models.Contact> contacts;
            if (startDate.HasValue && endDate.HasValue)
            {
                contacts = await _contactRepository.GetContactsByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                contacts = await _contactService.GetAllContactsAsync();
            }
            
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(contacts));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> GetContact(Guid id)
        {
            // Validate API credentials
            if (!Request.Headers.TryGetValue("X-Api-Key", out StringValues apiKey) ||
                !Request.Headers.TryGetValue("X-Api-Secret", out StringValues apiSecret))
            {
                return Unauthorized(new { Message = "API credentials are required" });
            }
            
            bool isValid = await _apiClientService.ValidateApiCredentialsAsync(apiKey, apiSecret);
            if (!isValid)
            {
                return Unauthorized(new { Message = "Invalid API credentials" });
            }
            
            // Get API client
            var apiClient = await _apiClientService.GetApiClientByApiKeyAsync(apiKey);
            
            // Log API usage
            await _apiClientService.LogApiUsageAsync(
                apiClient.Id,
                $"GET /api/external/contacts/{id}",
                true,
                $"id={id}",
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
            );
            
            var contact = await _contactService.GetContactByIdAsync(id);
            
            if (contact == null)
                return NotFound();
                
            return Ok(_mapper.Map<ContactDto>(contact));
        }
    }
}
