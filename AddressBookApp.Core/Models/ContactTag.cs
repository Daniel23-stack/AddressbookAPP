using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class ContactTag
    {
        public ContactTag()
        {
            Contacts = new List<Contact>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; } // Hex color code for UI display (e.g., "#28a745")
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}

