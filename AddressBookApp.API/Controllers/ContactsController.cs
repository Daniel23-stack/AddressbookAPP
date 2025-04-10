﻿﻿using System;
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
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(contacts));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> GetContact(Guid id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
                return NotFound();

            return Ok(_mapper.Map<ContactDto>(contact));
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> SearchContacts([FromQuery] string searchTerm)
        {
            var contacts = await _contactService.SearchContactsAsync(searchTerm);
            return Ok(_mapper.Map<IEnumerable<ContactDto>>(contacts));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContactDto>> CreateContact([FromBody] ContactCreateDto contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = _mapper.Map<Contact>(contactDto);
            var createdContact = await _contactService.CreateContactAsync(contact);

            var createdContactDto = _mapper.Map<ContactDto>(createdContact);
            return CreatedAtAction(nameof(GetContact), new { id = createdContactDto.Id }, createdContactDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactUpdateDto contactDto)
        {
            if (id != contactDto.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                await _contactService.UpdateContactAsync(contact);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                await _contactService.DeleteContactAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
