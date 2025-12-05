using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.Services
{
    public class SavedSearchService : ISavedSearchService
    {
        private readonly ISavedSearchRepository _savedSearchRepository;

        public SavedSearchService(ISavedSearchRepository savedSearchRepository)
        {
            _savedSearchRepository = savedSearchRepository;
        }

        public async Task<SavedSearch> CreateSavedSearchAsync(Guid userId, string name, string? description, SearchFilter filterCriteria)
        {
            var savedSearch = new SavedSearch
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UseCount = 0,
                FilterCriteria = JsonSerializer.Serialize(filterCriteria)
            };

            return await _savedSearchRepository.AddAsync(savedSearch);
        }

        public async Task<SavedSearch> UpdateSavedSearchAsync(Guid id, Guid userId, string name, string? description, SearchFilter filterCriteria)
        {
            var savedSearch = await _savedSearchRepository.GetByIdAndUserIdAsync(id, userId);
            if (savedSearch == null)
                throw new Exception($"Saved search with ID {id} not found");

            savedSearch.Name = name;
            savedSearch.Description = description;
            savedSearch.FilterCriteria = JsonSerializer.Serialize(filterCriteria);

            await _savedSearchRepository.UpdateAsync(savedSearch);
            return savedSearch;
        }

        public async Task DeleteSavedSearchAsync(Guid id, Guid userId)
        {
            var savedSearch = await _savedSearchRepository.GetByIdAndUserIdAsync(id, userId);
            if (savedSearch == null)
                throw new Exception($"Saved search with ID {id} not found");

            await _savedSearchRepository.DeleteAsync(savedSearch);
        }

        public async Task<IReadOnlyList<SavedSearch>> GetUserSavedSearchesAsync(Guid userId)
        {
            return await _savedSearchRepository.GetByUserIdAsync(userId);
        }

        public async Task<SavedSearch?> GetSavedSearchByIdAsync(Guid id, Guid userId)
        {
            return await _savedSearchRepository.GetByIdAndUserIdAsync(id, userId);
        }

        public async Task<SavedSearch> UseSavedSearchAsync(Guid id, Guid userId)
        {
            var savedSearch = await _savedSearchRepository.GetByIdAndUserIdAsync(id, userId);
            if (savedSearch == null)
                throw new Exception($"Saved search with ID {id} not found");

            await _savedSearchRepository.IncrementUseCountAsync(id);
            
            // Reload to get updated values
            return await _savedSearchRepository.GetByIdAndUserIdAsync(id, userId) ?? savedSearch;
        }
    }
}

