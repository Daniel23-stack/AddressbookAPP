using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly IApiClientRepository _apiClientRepository;
        private readonly IApiUsageRepository _apiUsageRepository;
        
        public ApiClientService(IApiClientRepository apiClientRepository, IApiUsageRepository apiUsageRepository)
        {
            _apiClientRepository = apiClientRepository;
            _apiUsageRepository = apiUsageRepository;
        }
        
        public async Task<ApiClient> GetApiClientByIdAsync(Guid id)
        {
            return await _apiClientRepository.GetByIdAsync(id);
        }
        
        public async Task<ApiClient> GetApiClientByApiKeyAsync(string apiKey)
        {
            return await _apiClientRepository.GetByApiKeyAsync(apiKey);
        }
        
        public async Task<IReadOnlyList<ApiClient>> GetAllApiClientsAsync()
        {
            return await _apiClientRepository.ListAllAsync();
        }
        
        public async Task<ApiClient> CreateApiClientAsync(string name)
        {
            var apiKey = GenerateApiKey();
            var apiSecret = GenerateApiSecret();
            
            var apiClient = new ApiClient
            {
                Id = Guid.NewGuid(),
                Name = name,
                ApiKey = apiKey,
                ApiSecret = apiSecret,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            return await _apiClientRepository.AddAsync(apiClient);
        }
        
        public async Task UpdateApiClientAsync(ApiClient apiClient)
        {
            apiClient.LastUsed = DateTime.UtcNow;
            await _apiClientRepository.UpdateAsync(apiClient);
        }
        
        public async Task DeleteApiClientAsync(Guid id)
        {
            var apiClient = await _apiClientRepository.GetByIdAsync(id);
            if (apiClient == null)
                throw new Exception($"API Client with ID {id} not found");
                
            apiClient.IsActive = false;
            await _apiClientRepository.UpdateAsync(apiClient);
        }
        
        public async Task<bool> ValidateApiCredentialsAsync(string apiKey, string apiSecret)
        {
            return await _apiClientRepository.ValidateApiCredentialsAsync(apiKey, apiSecret);
        }
        
        public async Task LogApiUsageAsync(Guid apiClientId, string endpoint, bool isSuccessful, string parameters, string ipAddress)
        {
            var apiUsage = new ApiUsage
            {
                Id = Guid.NewGuid(),
                ApiClientId = apiClientId,
                Endpoint = endpoint,
                Timestamp = DateTime.UtcNow,
                IsSuccessful = isSuccessful,
                Parameters = parameters,
                IpAddress = ipAddress
            };
            
            await _apiUsageRepository.AddAsync(apiUsage);
            
            var apiClient = await _apiClientRepository.GetByIdAsync(apiClientId);
            if (apiClient != null)
            {
                apiClient.LastUsed = DateTime.UtcNow;
                await _apiClientRepository.UpdateAsync(apiClient);
            }
        }
        
        private string GenerateApiKey()
        {
            return Guid.NewGuid().ToString("N");
        }
        
        private string GenerateApiSecret()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
