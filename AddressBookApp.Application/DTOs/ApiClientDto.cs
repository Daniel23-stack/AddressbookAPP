using System;

namespace AddressBookApp.Application.DTOs
{
    public class ApiClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
    }
    
    public class ApiClientCreateDto
    {
        public string Name { get; set; }
    }
    
    public class ApiClientCredentialsDto
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
