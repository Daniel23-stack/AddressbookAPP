using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class CustomFieldDefinition
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public CustomFieldType Type { get; set; }
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }
        public List<string>? Options { get; set; } // For dropdown/checkbox types
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public ICollection<CustomFieldValue> Values { get; set; } = new List<CustomFieldValue>();
    }

    public enum CustomFieldType
    {
        Text = 0,
        Number = 1,
        Date = 2,
        DateTime = 3,
        Boolean = 4,
        Dropdown = 5,
        MultiSelect = 6,
        Email = 7,
        Phone = 8,
        Url = 9,
        TextArea = 10
    }
}

