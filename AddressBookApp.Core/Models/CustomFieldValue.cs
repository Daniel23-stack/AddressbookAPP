using System;

namespace AddressBookApp.Core.Models
{
    public class CustomFieldValue
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid FieldDefinitionId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Contact? Contact { get; set; }
        public CustomFieldDefinition? FieldDefinition { get; set; }
    }
}

