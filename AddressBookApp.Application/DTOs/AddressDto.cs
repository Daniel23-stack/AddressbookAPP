using System;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.DTOs
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public AddressType Type { get; set; }
    }
    
    public class AddressCreateDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public AddressType Type { get; set; }
    }
    
    public class AddressUpdateDto
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public AddressType Type { get; set; }
    }
}
