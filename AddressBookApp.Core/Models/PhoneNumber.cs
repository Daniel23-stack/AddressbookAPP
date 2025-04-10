using System;

namespace AddressBookApp.Core.Models
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
    }
    
    public enum PhoneType
    {
        Mobile,
        Home,
        Work,
        Fax,
        Other
    }
}
