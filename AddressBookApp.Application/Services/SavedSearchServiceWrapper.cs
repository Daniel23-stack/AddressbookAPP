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
    // Wrapper service that works with DTOs - used by API controllers
    public class SavedSearchServiceWrapper
    {
        private readonly ISavedSearchService _savedSearchService;

        public SavedSearchServiceWrapper(ISavedSearchService savedSearchService)
        {
            _savedSearchService = savedSearchService;
        }

        public async Task<SavedSearchDto> CreateSavedSearchAsync(Guid userId, CreateSavedSearchDto dto)
        {
            var filter = MapToSearchFilter(dto.FilterCriteria);
            var savedSearch = await _savedSearchService.CreateSavedSearchAsync(userId, dto.Name, dto.Description, filter);
            return MapToDto(savedSearch);
        }

        public async Task<SavedSearchDto> UpdateSavedSearchAsync(Guid id, Guid userId, UpdateSavedSearchDto dto)
        {
            var filter = MapToSearchFilter(dto.FilterCriteria);
            var savedSearch = await _savedSearchService.UpdateSavedSearchAsync(id, userId, dto.Name, dto.Description, filter);
            return MapToDto(savedSearch);
        }

        public async Task DeleteSavedSearchAsync(Guid id, Guid userId)
        {
            await _savedSearchService.DeleteSavedSearchAsync(id, userId);
        }

        public async Task<IReadOnlyList<SavedSearchDto>> GetUserSavedSearchesAsync(Guid userId)
        {
            var savedSearches = await _savedSearchService.GetUserSavedSearchesAsync(userId);
            return savedSearches.Select(MapToDto).ToList();
        }

        public async Task<SavedSearchDto?> GetSavedSearchByIdAsync(Guid id, Guid userId)
        {
            var savedSearch = await _savedSearchService.GetSavedSearchByIdAsync(id, userId);
            return savedSearch != null ? MapToDto(savedSearch) : null;
        }

        public async Task<SavedSearchDto> UseSavedSearchAsync(Guid id, Guid userId)
        {
            var savedSearch = await _savedSearchService.UseSavedSearchAsync(id, userId);
            return MapToDto(savedSearch);
        }

        private SavedSearchDto MapToDto(SavedSearch savedSearch)
        {
            SearchFilterDto? filterCriteria = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(savedSearch.FilterCriteria))
                {
                    var filter = JsonSerializer.Deserialize<SearchFilter>(savedSearch.FilterCriteria);
                    if (filter != null)
                    {
                        filterCriteria = new SearchFilterDto
                        {
                            SearchTerm = filter.SearchTerm,
                            CreatedAfter = filter.CreatedAfter,
                            CreatedBefore = filter.CreatedBefore,
                            UpdatedAfter = filter.UpdatedAfter,
                            UpdatedBefore = filter.UpdatedBefore,
                            Company = filter.Company,
                            HasEmail = filter.HasEmail,
                            HasPhone = filter.HasPhone,
                            HasAddress = filter.HasAddress,
                            IsClient = filter.IsClient,
                            IsImported = filter.IsImported,
                            GroupIds = filter.GroupIds,
                            TagIds = filter.TagIds,
                            SearchInNotes = filter.SearchInNotes,
                            EmailDomain = filter.EmailDomain,
                            PhoneNumber = filter.PhoneNumber
                        };
                    }
                }
            }
            catch
            {
                filterCriteria = new SearchFilterDto();
            }

            return new SavedSearchDto
            {
                Id = savedSearch.Id,
                Name = savedSearch.Name,
                Description = savedSearch.Description,
                UserId = savedSearch.UserId,
                CreatedAt = savedSearch.CreatedAt,
                LastUsedAt = savedSearch.LastUsedAt,
                UseCount = savedSearch.UseCount,
                FilterCriteria = filterCriteria ?? new SearchFilterDto()
            };
        }

        private SearchFilter MapToSearchFilter(SearchFilterDto dto)
        {
            return new SearchFilter
            {
                SearchTerm = dto.SearchTerm,
                CreatedAfter = dto.CreatedAfter,
                CreatedBefore = dto.CreatedBefore,
                UpdatedAfter = dto.UpdatedAfter,
                UpdatedBefore = dto.UpdatedBefore,
                Company = dto.Company,
                HasEmail = dto.HasEmail,
                HasPhone = dto.HasPhone,
                HasAddress = dto.HasAddress,
                IsClient = dto.IsClient,
                IsImported = dto.IsImported,
                GroupIds = dto.GroupIds,
                TagIds = dto.TagIds,
                SearchInNotes = dto.SearchInNotes,
                EmailDomain = dto.EmailDomain,
                PhoneNumber = dto.PhoneNumber
            };
        }
    }
}

