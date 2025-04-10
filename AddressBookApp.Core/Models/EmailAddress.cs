using System;

namespace AddressBookApp.Core.Models
{
    public class EmailAddress
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public EmailType Type { get; set; }
        
        public Guid ContactId { get; set; }
        public Contact? Contact { get; set; }
    }
    
    public enum EmailType
    {
        Personal,
        Work,
        Other
    }
}
