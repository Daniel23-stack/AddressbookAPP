using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IApiUsageRepository : IRepository<ApiUsage>
    {
        Task<int> GetUsageCountInLastDaysAsync(int days);
        Task<int> GetUniqueClientCountInLastDaysAsync(int days);
        Task<IReadOnlyList<ApiUsage>> GetUsagesByClientIdAsync(Guid clientId);
        Task<IReadOnlyList<ApiUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
