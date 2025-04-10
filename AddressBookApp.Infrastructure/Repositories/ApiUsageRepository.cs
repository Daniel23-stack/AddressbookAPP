using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class ApiUsageRepository : Repository<ApiUsage>, IApiUsageRepository
    {
        public ApiUsageRepository(AddressBookContext context) : base(context)
        {
        }
        
        public async Task<int> GetUsageCountInLastDaysAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.ApiUsages
                .CountAsync(u => u.Timestamp >= cutoffDate);
        }
        
        public async Task<int> GetUniqueClientCountInLastDaysAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.ApiUsages
                .Where(u => u.Timestamp >= cutoffDate)
                .Select(u => u.ApiClientId)
                .Distinct()
                .CountAsync();
        }
        
        public async Task<IReadOnlyList<ApiUsage>> GetUsagesByClientIdAsync(Guid clientId)
        {
            return await _context.ApiUsages
                .Where(u => u.ApiClientId == clientId)
                .OrderByDescending(u => u.Timestamp)
                .ToListAsync();
        }
        
        public async Task<IReadOnlyList<ApiUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ApiUsages
                .Where(u => u.Timestamp >= startDate && u.Timestamp <= endDate)
                .OrderByDescending(u => u.Timestamp)
                .ToListAsync();
        }
    }
}
