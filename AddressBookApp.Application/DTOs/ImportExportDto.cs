using System;

namespace AddressBookApp.Application.DTOs
{
    public class DataExportDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int RecordCount { get; set; }
        public DateTime ExportedAt { get; set; }
        public string ExportType { get; set; }
    }
    
    public class ImportHistoryDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public DateTime ImportedAt { get; set; }
    }
    
    public class ImportResultDto
    {
        public bool Success { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public string Message { get; set; }
    }
}
