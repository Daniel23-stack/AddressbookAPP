using System;

namespace AddressBookApp.Core.Models
{
    public class ApiUsage
    {
        public Guid Id { get; set; }
        public Guid ApiClientId { get; set; }
        public ApiClient ApiClient { get; set; }
        public string Endpoint { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }
        public string Parameters { get; set; }
        public string IpAddress { get; set; }
    }
}
