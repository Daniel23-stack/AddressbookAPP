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

        public async Task<IReadOnlyList<Contact>> AdvancedSearchAsync(SearchFilter filter)
        {
            var query = _context.Contacts
                .Include(c => c.Addresses)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Groups)
                .Include(c => c.Tags)
                .AsQueryable();

            // Search term - search in name, company, notes, email, phone
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(c =>
                    (c.FirstName != null && c.FirstName.ToLower().Contains(searchTerm)) ||
                    (c.LastName != null && c.LastName.ToLower().Contains(searchTerm)) ||
                    (c.Company != null && c.Company.ToLower().Contains(searchTerm)) ||
                    (filter.SearchInNotes == true && c.QuickNotes != null && c.QuickNotes.ToLower().Contains(searchTerm)) ||
                    c.EmailAddresses.Any(e => e.Email != null && e.Email.ToLower().Contains(searchTerm)) ||
                    c.PhoneNumbers.Any(p => p.Number != null && p.Number.Contains(searchTerm)));
            }

            // Date range filters
            if (filter.CreatedAfter.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= filter.CreatedAfter.Value);
            }

            if (filter.CreatedBefore.HasValue)
            {
                query = query.Where(c => c.CreatedAt <= filter.CreatedBefore.Value);
            }

            if (filter.UpdatedAfter.HasValue)
            {
                query = query.Where(c => c.UpdatedAt.HasValue && c.UpdatedAt >= filter.UpdatedAfter.Value);
            }

            if (filter.UpdatedBefore.HasValue)
            {
                query = query.Where(c => c.UpdatedAt.HasValue && c.UpdatedAt <= filter.UpdatedBefore.Value);
            }

            // Company filter
            if (!string.IsNullOrWhiteSpace(filter.Company))
            {
                query = query.Where(c => c.Company != null && c.Company.ToLower().Contains(filter.Company.ToLower()));
            }

            // Has email filter
            if (filter.HasEmail.HasValue)
            {
                if (filter.HasEmail.Value)
                {
                    query = query.Where(c => c.EmailAddresses.Any());
                }
                else
                {
                    query = query.Where(c => !c.EmailAddresses.Any());
                }
            }

            // Has phone filter
            if (filter.HasPhone.HasValue)
            {
                if (filter.HasPhone.Value)
                {
                    query = query.Where(c => c.PhoneNumbers.Any());
                }
                else
                {
                    query = query.Where(c => !c.PhoneNumbers.Any());
                }
            }

            // Has address filter
            if (filter.HasAddress.HasValue)
            {
                if (filter.HasAddress.Value)
                {
                    query = query.Where(c => c.Addresses.Any());
                }
                else
                {
                    query = query.Where(c => !c.Addresses.Any());
                }
            }

            // IsClient filter
            if (filter.IsClient.HasValue)
            {
                query = query.Where(c => c.IsClient == filter.IsClient.Value);
            }

            // IsImported filter
            if (filter.IsImported.HasValue)
            {
                query = query.Where(c => c.IsImported == filter.IsImported.Value);
            }

            // Group filter
            if (filter.GroupIds != null && filter.GroupIds.Any())
            {
                query = query.Where(c => c.Groups.Any(g => filter.GroupIds.Contains(g.Id)));
            }

            // Tag filter
            if (filter.TagIds != null && filter.TagIds.Any())
            {
                query = query.Where(c => c.Tags.Any(t => filter.TagIds.Contains(t.Id)));
            }

            // Email domain filter
            if (!string.IsNullOrWhiteSpace(filter.EmailDomain))
            {
                var domain = filter.EmailDomain.ToLower();
                query = query.Where(c => c.EmailAddresses.Any(e => e.Email != null && e.Email.ToLower().EndsWith("@" + domain)));
            }

            // Phone number filter
            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumbers.Any(p => p.Number != null && p.Number.Contains(filter.PhoneNumber)));
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetAdvancedSearchCountAsync(SearchFilter filter)
        {
            var query = _context.Contacts.AsQueryable();

            // Apply same filters as AdvancedSearchAsync but without includes for performance
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(c =>
                    (c.FirstName != null && c.FirstName.ToLower().Contains(searchTerm)) ||
                    (c.LastName != null && c.LastName.ToLower().Contains(searchTerm)) ||
                    (c.Company != null && c.Company.ToLower().Contains(searchTerm)) ||
                    (filter.SearchInNotes == true && c.QuickNotes != null && c.QuickNotes.ToLower().Contains(searchTerm)) ||
                    c.EmailAddresses.Any(e => e.Email != null && e.Email.ToLower().Contains(searchTerm)) ||
                    c.PhoneNumbers.Any(p => p.Number != null && p.Number.Contains(searchTerm)));
            }

            if (filter.CreatedAfter.HasValue)
                query = query.Where(c => c.CreatedAt >= filter.CreatedAfter.Value);
            if (filter.CreatedBefore.HasValue)
                query = query.Where(c => c.CreatedAt <= filter.CreatedBefore.Value);
            if (filter.UpdatedAfter.HasValue)
                query = query.Where(c => c.UpdatedAt.HasValue && c.UpdatedAt >= filter.UpdatedAfter.Value);
            if (filter.UpdatedBefore.HasValue)
                query = query.Where(c => c.UpdatedAt.HasValue && c.UpdatedAt <= filter.UpdatedBefore.Value);
            if (!string.IsNullOrWhiteSpace(filter.Company))
                query = query.Where(c => c.Company != null && c.Company.ToLower().Contains(filter.Company.ToLower()));
            if (filter.HasEmail.HasValue)
                query = query.Where(c => filter.HasEmail.Value ? c.EmailAddresses.Any() : !c.EmailAddresses.Any());
            if (filter.HasPhone.HasValue)
                query = query.Where(c => filter.HasPhone.Value ? c.PhoneNumbers.Any() : !c.PhoneNumbers.Any());
            if (filter.HasAddress.HasValue)
                query = query.Where(c => filter.HasAddress.Value ? c.Addresses.Any() : !c.Addresses.Any());
            if (filter.IsClient.HasValue)
                query = query.Where(c => c.IsClient == filter.IsClient.Value);
            if (filter.IsImported.HasValue)
                query = query.Where(c => c.IsImported == filter.IsImported.Value);
            if (filter.GroupIds != null && filter.GroupIds.Any())
                query = query.Where(c => c.Groups.Any(g => filter.GroupIds.Contains(g.Id)));
            if (filter.TagIds != null && filter.TagIds.Any())
                query = query.Where(c => c.Tags.Any(t => filter.TagIds.Contains(t.Id)));
            if (!string.IsNullOrWhiteSpace(filter.EmailDomain))
            {
                var domain = filter.EmailDomain.ToLower();
                query = query.Where(c => c.EmailAddresses.Any(e => e.Email != null && e.Email.ToLower().EndsWith("@" + domain)));
            }
            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                query = query.Where(c => c.PhoneNumbers.Any(p => p.Number != null && p.Number.Contains(filter.PhoneNumber)));

            return await query.CountAsync();
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
