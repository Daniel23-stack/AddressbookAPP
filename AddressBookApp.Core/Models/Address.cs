using System;

namespace AddressBookApp.Core.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public AddressType Type { get; set; }
        
        public Guid ContactId { get; set; }
        public Contact? Contact { get; set; }
    }
    
    public enum AddressType
    {
        Home,
        Work,
        Other
    }
}
