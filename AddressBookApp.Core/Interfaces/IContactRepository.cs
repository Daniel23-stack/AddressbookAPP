﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<Contact> GetContactWithDetailsAsync(Guid id);
        Task<IReadOnlyList<Contact>> SearchContactsAsync(string searchTerm);
        Task<IReadOnlyList<Contact>> GetClientContactsAsync();
        Task<IReadOnlyList<Contact>> GetImportedContactsAsync();
        Task<IReadOnlyList<Contact>> GetContactsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetClientCountAsync();
        Task<int> GetImportedContactsCountAsync();
    }
}
