using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            return await _contactRepository.GetContactWithDetailsAsync(id);
        }
        
        public async Task<IReadOnlyList<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.ListAllAsync();
        }
        
        public async Task<IReadOnlyList<Contact>> SearchContactsAsync(string searchTerm)
        {
            return await _contactRepository.SearchContactsAsync(searchTerm);
        }
        
        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            contact.Id = Guid.NewGuid();
            contact.CreatedAt = DateTime.UtcNow;
            
            return await _contactRepository.AddAsync(contact);
        }
        
        public async Task UpdateContactAsync(Contact contact)
        {
            var existingContact = await _contactRepository.GetByIdAsync(contact.Id);
            if (existingContact == null)
                throw new Exception($"Contact with ID {contact.Id} not found");
                
            contact.UpdatedAt = DateTime.UtcNow;
            await _contactRepository.UpdateAsync(contact);
        }
        
        public async Task DeleteContactAsync(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null)
                throw new Exception($"Contact with ID {id} not found");
                
            await _contactRepository.DeleteAsync(contact);
        }
    }
}
