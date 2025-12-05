using System;

namespace AddressBookApp.Core.Models
{
    public class ContactNote
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty; // HTML or Markdown
        public NoteType Type { get; set; }
        public string? Category { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Contact? Contact { get; set; }
        public User? User { get; set; }
    }

    public enum NoteType
    {
        General = 0,
        Call = 1,
        Meeting = 2,
        FollowUp = 3,
        Important = 4,
        Reminder = 5
    }
}

