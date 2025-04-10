using System;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.DTOs
{
    public class EmailAddressDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public EmailType Type { get; set; }
    }
    
    public class EmailAddressCreateDto
    {
        public string Email { get; set; }
        public EmailType Type { get; set; }
    }
    
    public class EmailAddressUpdateDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public EmailType Type { get; set; }
    }
}
