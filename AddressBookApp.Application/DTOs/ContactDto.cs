﻿﻿﻿using System;
using System.Collections.Generic;

namespace AddressBookApp.Application.DTOs
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
        public bool IsClient { get; set; }
        public bool IsImported { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ImportedById { get; set; }

        public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
        public ICollection<PhoneNumberDto> PhoneNumbers { get; set; } = new List<PhoneNumberDto>();
        public ICollection<EmailAddressDto> EmailAddresses { get; set; } = new List<EmailAddressDto>();
    }

    public class ContactCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
        public bool IsClient { get; set; }

        public ICollection<AddressCreateDto> Addresses { get; set; } = new List<AddressCreateDto>();
        public ICollection<PhoneNumberCreateDto> PhoneNumbers { get; set; } = new List<PhoneNumberCreateDto>();
        public ICollection<EmailAddressCreateDto> EmailAddresses { get; set; } = new List<EmailAddressCreateDto>();
    }

    public class ContactUpdateDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
        public bool IsClient { get; set; }

        public ICollection<AddressUpdateDto> Addresses { get; set; } = new List<AddressUpdateDto>();
        public ICollection<PhoneNumberUpdateDto> PhoneNumbers { get; set; } = new List<PhoneNumberUpdateDto>();
        public ICollection<EmailAddressUpdateDto> EmailAddresses { get; set; } = new List<EmailAddressUpdateDto>();
    }
}
