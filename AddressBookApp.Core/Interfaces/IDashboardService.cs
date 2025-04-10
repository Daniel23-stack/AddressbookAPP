using System;
using System.Threading.Tasks;

namespace AddressBookApp.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetTotalContactsCountAsync();
        Task<int> GetClientContactsCountAsync();
        Task<int> GetImportedContactsCountAsync();
        Task<int> GetDataExportCountAsync();
        Task<int> GetApiClientUsageCountLastDaysAsync(int days);
    }
}
