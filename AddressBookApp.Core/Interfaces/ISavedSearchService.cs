using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface ISavedSearchService
    {
        Task<SavedSearch> CreateSavedSearchAsync(Guid userId, string name, string? description, SearchFilter filterCriteria);
        Task<SavedSearch> UpdateSavedSearchAsync(Guid id, Guid userId, string name, string? description, SearchFilter filterCriteria);
        Task DeleteSavedSearchAsync(Guid id, Guid userId);
        Task<IReadOnlyList<SavedSearch>> GetUserSavedSearchesAsync(Guid userId);
        Task<SavedSearch?> GetSavedSearchByIdAsync(Guid id, Guid userId);
        Task<SavedSearch> UseSavedSearchAsync(Guid id, Guid userId);
    }
}

