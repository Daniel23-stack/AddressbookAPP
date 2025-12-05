using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class SavedSearch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public int UseCount { get; set; }

        // Store filter criteria as JSON string
        public string FilterCriteria { get; set; } = string.Empty;

        // Navigation properties
        public User? User { get; set; }
    }
}

