using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class ContactGroup
    {
        public ContactGroup()
        {
            Contacts = new List<Contact>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; } // Hex color code for UI display (e.g., "#667eea")
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}

