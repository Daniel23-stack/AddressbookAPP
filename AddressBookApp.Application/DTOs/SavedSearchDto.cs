using System;

namespace AddressBookApp.Application.DTOs
{
    public class SavedSearchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public int UseCount { get; set; }
        public SearchFilterDto FilterCriteria { get; set; } = new SearchFilterDto();
    }

    public class CreateSavedSearchDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SearchFilterDto FilterCriteria { get; set; } = new SearchFilterDto();
    }

    public class UpdateSavedSearchDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SearchFilterDto FilterCriteria { get; set; } = new SearchFilterDto();
    }
}

