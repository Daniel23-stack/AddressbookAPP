using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class ApiClient
    {
        public ApiClient()
        {
            ApiUsages = new List<ApiUsage>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
        
        public ICollection<ApiUsage> ApiUsages { get; set; }
    }
}
