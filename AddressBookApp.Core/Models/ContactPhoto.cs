using System;

namespace AddressBookApp.Core.Models
{
    public class ContactPhoto
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime UploadedAt { get; set; }
        public Guid? UploadedById { get; set; }

        // Navigation properties
        public Contact? Contact { get; set; }
        public User? UploadedBy { get; set; }
    }
}

