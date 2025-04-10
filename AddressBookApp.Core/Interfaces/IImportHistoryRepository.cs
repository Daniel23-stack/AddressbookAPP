using System;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IImportHistoryRepository : IRepository<ImportHistory>
    {
        Task<int> GetImportCountAsync();
        Task<int> GetImportedContactsCountAsync();
        Task<int> GetImportCountByUserAsync(Guid userId);
    }
}
