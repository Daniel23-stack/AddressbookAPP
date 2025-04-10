using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IApiClientRepository : IRepository<ApiClient>
    {
        Task<ApiClient> GetByApiKeyAsync(string apiKey);
        Task<bool> ValidateApiCredentialsAsync(string apiKey, string apiSecret);
    }
}
