using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiClientsController : ControllerBase
    {
        private readonly IApiClientService _apiClientService;
        private readonly IMapper _mapper;
        
        public ApiClientsController(IApiClientService apiClientService, IMapper mapper)
        {
            _apiClientService = apiClientService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApiClientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ApiClientDto>>> GetApiClients()
        {
            var apiClients = await _apiClientService.GetAllApiClientsAsync();
            return Ok(_mapper.Map<IEnumerable<ApiClientDto>>(apiClients));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiClientDto>> GetApiClient(Guid id)
        {
            var apiClient = await _apiClientService.GetApiClientByIdAsync(id);
            
            if (apiClient == null)
                return NotFound();
                
            return Ok(_mapper.Map<ApiClientDto>(apiClient));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ApiClientCredentialsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiClientCredentialsDto>> CreateApiClient([FromBody] ApiClientCreateDto apiClientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var apiClient = await _apiClientService.CreateApiClientAsync(apiClientDto.Name);
            
            var credentials = new ApiClientCredentialsDto
            {
                ApiKey = apiClient.ApiKey,
                ApiSecret = apiClient.ApiSecret
            };
            
            return CreatedAtAction(nameof(GetApiClient), new { id = apiClient.Id }, credentials);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteApiClient(Guid id)
        {
            try
            {
                await _apiClientService.DeleteApiClientAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
