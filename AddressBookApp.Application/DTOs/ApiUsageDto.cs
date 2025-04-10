using System;

namespace AddressBookApp.Application.DTOs
{
    public class ApiUsageDto
    {
        public Guid Id { get; set; }
        public Guid ApiClientId { get; set; }
        public string ApiClientName { get; set; }
        public string Endpoint { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }
        public string Parameters { get; set; }
        public string IpAddress { get; set; }
    }
}
