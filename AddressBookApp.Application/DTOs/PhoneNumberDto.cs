using System;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.DTOs
{
    public class PhoneNumberDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }
    
    public class PhoneNumberCreateDto
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }
    
    public class PhoneNumberUpdateDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }
}
