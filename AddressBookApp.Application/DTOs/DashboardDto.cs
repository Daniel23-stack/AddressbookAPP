namespace AddressBookApp.Application.DTOs
{
    public class DashboardDto
    {
        public int TotalContacts { get; set; }
        public int ClientContacts { get; set; }
        public int ImportedContacts { get; set; }
        public int DataExportCount { get; set; }
        public int ApiClientUsageCount { get; set; }
    }
}
