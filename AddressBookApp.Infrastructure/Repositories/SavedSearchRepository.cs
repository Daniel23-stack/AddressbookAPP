using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class SavedSearchRepository : Repository<SavedSearch>, ISavedSearchRepository
    {
        public SavedSearchRepository(AddressBookContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<SavedSearch>> GetByUserIdAsync(Guid userId)
        {
            return await _context.SavedSearches!
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.LastUsedAt ?? s.CreatedAt)
                .ToListAsync();
        }

        public async Task<SavedSearch?> GetByIdAndUserIdAsync(Guid id, Guid userId)
        {
            return await _context.SavedSearches!
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        }

        public async Task IncrementUseCountAsync(Guid id)
        {
            var savedSearch = await _context.SavedSearches!.FindAsync(id);
            if (savedSearch != null)
            {
                savedSearch.UseCount++;
                savedSearch.LastUsedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}

