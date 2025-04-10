using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IContactService
    {
        Task<Contact> GetContactByIdAsync(Guid id);
        Task<IReadOnlyList<Contact>> GetAllContactsAsync();
        Task<IReadOnlyList<Contact>> SearchContactsAsync(string searchTerm);
        Task<Contact> CreateContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(Guid id);
    }
}
