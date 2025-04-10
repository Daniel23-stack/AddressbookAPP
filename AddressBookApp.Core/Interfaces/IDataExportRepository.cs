using System;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IDataExportRepository : IRepository<DataExport>
    {
        Task<int> GetExportCountAsync();
        Task<int> GetExportCountByUserAsync(Guid userId);
    }
}
