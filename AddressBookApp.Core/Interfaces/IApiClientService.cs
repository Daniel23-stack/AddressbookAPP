using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IApiClientService
    {
        Task<ApiClient> GetApiClientByIdAsync(Guid id);
        Task<ApiClient> GetApiClientByApiKeyAsync(string apiKey);
        Task<IReadOnlyList<ApiClient>> GetAllApiClientsAsync();
        Task<ApiClient> CreateApiClientAsync(string name);
        Task UpdateApiClientAsync(ApiClient apiClient);
        Task DeleteApiClientAsync(Guid id);
        Task<bool> ValidateApiCredentialsAsync(string apiKey, string apiSecret);
        Task LogApiUsageAsync(Guid apiClientId, string endpoint, bool isSuccessful, string parameters, string ipAddress);
    }
}
