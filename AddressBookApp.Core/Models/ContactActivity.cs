using System;

namespace AddressBookApp.Core.Models
{
    public class ContactActivity
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid? UserId { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public Contact? Contact { get; set; }
        public User? User { get; set; }
    }

    public enum ActivityType
    {
        Created = 0,
        Updated = 1,
        Deleted = 2,
        Viewed = 3,
        Exported = 4,
        Imported = 5,
        ApiAccessed = 6,
        PhotoUploaded = 7,
        PhotoDeleted = 8,
        NoteAdded = 9,
        NoteUpdated = 10,
        NoteDeleted = 11,
        GroupAssigned = 12,
        GroupRemoved = 13,
        TagAssigned = 14,
        TagRemoved = 15
    }
}

