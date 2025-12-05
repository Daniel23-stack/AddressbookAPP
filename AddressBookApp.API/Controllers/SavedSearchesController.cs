using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Application.Services;
using AddressBookApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavedSearchesController : ControllerBase
    {
        private readonly SavedSearchServiceWrapper _savedSearchService;

        public SavedSearchesController(SavedSearchServiceWrapper savedSearchService)
        {
            _savedSearchService = savedSearchService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SavedSearchDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SavedSearchDto>>> GetSavedSearches()
        {
            var userId = GetCurrentUserId();
            var savedSearches = await _savedSearchService.GetUserSavedSearchesAsync(userId);
            return Ok(savedSearches);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SavedSearchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SavedSearchDto>> GetSavedSearch(Guid id)
        {
            var userId = GetCurrentUserId();
            var savedSearch = await _savedSearchService.GetSavedSearchByIdAsync(id, userId);
            
            if (savedSearch == null)
                return NotFound();

            return Ok(savedSearch);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SavedSearchDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SavedSearchDto>> CreateSavedSearch([FromBody] CreateSavedSearchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var savedSearch = await _savedSearchService.CreateSavedSearchAsync(userId, dto);
            
            return CreatedAtAction(nameof(GetSavedSearch), new { id = savedSearch.Id }, savedSearch);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SavedSearchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SavedSearchDto>> UpdateSavedSearch(Guid id, [FromBody] UpdateSavedSearchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var savedSearch = await _savedSearchService.UpdateSavedSearchAsync(id, userId, dto);
                return Ok(savedSearch);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSavedSearch(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _savedSearchService.DeleteSavedSearchAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("{id}/use")]
        [ProducesResponseType(typeof(SavedSearchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SavedSearchDto>> UseSavedSearch(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var savedSearch = await _savedSearchService.UseSavedSearchAsync(id, userId);
                return Ok(savedSearch);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found");
            }
            return userId;
        }
    }
}

