using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class ApiClientRepository : Repository<ApiClient>, IApiClientRepository
    {
        public ApiClientRepository(AddressBookContext context) : base(context)
        {
        }
        
        public async Task<ApiClient> GetByApiKeyAsync(string apiKey)
        {
            return await _context.ApiClients
                .FirstOrDefaultAsync(c => c.ApiKey == apiKey && c.IsActive);
        }
        
        public async Task<bool> ValidateApiCredentialsAsync(string apiKey, string apiSecret)
        {
            return await _context.ApiClients
                .AnyAsync(c => c.ApiKey == apiKey && c.ApiSecret == apiSecret && c.IsActive);
        }
    }
}
