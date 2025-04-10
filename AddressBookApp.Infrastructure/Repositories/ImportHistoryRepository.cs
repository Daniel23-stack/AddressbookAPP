using System;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class ImportHistoryRepository : Repository<ImportHistory>, IImportHistoryRepository
    {
        public ImportHistoryRepository(AddressBookContext context) : base(context)
        {
        }
        
        public async Task<int> GetImportCountAsync()
        {
            return await _context.ImportHistories.CountAsync();
        }
        
        public async Task<int> GetImportedContactsCountAsync()
        {
            return await _context.ImportHistories.SumAsync(i => i.SuccessfulRecords);
        }
        
        public async Task<int> GetImportCountByUserAsync(Guid userId)
        {
            return await _context.ImportHistories
                .CountAsync(i => i.UserId == userId);
        }
    }
}
