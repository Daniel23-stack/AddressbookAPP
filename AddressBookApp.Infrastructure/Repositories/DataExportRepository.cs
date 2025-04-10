using System;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Repositories
{
    public class DataExportRepository : Repository<DataExport>, IDataExportRepository
    {
        public DataExportRepository(AddressBookContext context) : base(context)
        {
        }
        
        public async Task<int> GetExportCountAsync()
        {
            return await _context.DataExports.CountAsync();
        }
        
        public async Task<int> GetExportCountByUserAsync(Guid userId)
        {
            return await _context.DataExports
                .CountAsync(e => e.UserId == userId);
        }
    }
}
