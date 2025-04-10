using System;

namespace AddressBookApp.Core.Models
{
    public class DataExport
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? FileName { get; set; }
        public int RecordCount { get; set; }
        public DateTime ExportedAt { get; set; }
        public ExportType Type { get; set; }
    }
    
    public enum ExportType
    {
        Csv,
        Json,
        Xml
    }
}
