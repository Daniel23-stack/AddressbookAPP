using System;

namespace AddressBookApp.Core.Models
{
    public class ImportHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string FileName { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public DateTime ImportedAt { get; set; }
    }
}
