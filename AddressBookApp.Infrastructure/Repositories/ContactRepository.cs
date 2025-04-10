﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(AddressBookContext context) : base(context)
        {
        }

        public async Task<Contact> GetContactWithDetailsAsync(Guid id)
        {
            return await _context.Contacts
                .Include(c => c.Addresses)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IReadOnlyList<Contact>> SearchContactsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await ListAllAsync();

            searchTerm = searchTerm.ToLower();

            return await _context.Contacts
                .Where(c => c.FirstName.ToLower().Contains(searchTerm) ||
                           c.LastName.ToLower().Contains(searchTerm) ||
                           c.Company.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contact>> GetClientContactsAsync()
        {
            return await _context.Contacts
                .Where(c => c.IsClient)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contact>> GetImportedContactsAsync()
        {
            return await _context.Contacts
                .Where(c => c.IsImported)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contact>> GetContactsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Contacts
                .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<int> GetClientCountAsync()
        {
            return await _context.Contacts
                .CountAsync(c => c.IsClient);
        }

        public async Task<int> GetImportedContactsCountAsync()
        {
            return await _context.Contacts
                .CountAsync(c => c.IsImported);
        }
    }
}
