﻿using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class Contact
    {
        public Contact()
        {
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
            Groups = new List<ContactGroup>();
            Tags = new List<ContactTag>();
        }

        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? QuickNotes { get; set; } // Simple text notes (legacy)
        public bool IsClient { get; set; }
        public bool IsImported { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ImportedById { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<EmailAddress> EmailAddresses { get; set; }
        public ICollection<ContactGroup> Groups { get; set; }
        public ICollection<ContactTag> Tags { get; set; }
        public ICollection<ContactActivity> Activities { get; set; } = new List<ContactActivity>();
        public ICollection<ContactPhoto> Photos { get; set; } = new List<ContactPhoto>();
        public ICollection<ContactNote> Notes { get; set; } = new List<ContactNote>();
        public ICollection<CustomFieldValue> CustomFieldValues { get; set; } = new List<CustomFieldValue>();
    }
}
