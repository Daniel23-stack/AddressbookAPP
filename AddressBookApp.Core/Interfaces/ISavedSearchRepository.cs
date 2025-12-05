using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface ISavedSearchRepository : IRepository<SavedSearch>
    {
        Task<IReadOnlyList<SavedSearch>> GetByUserIdAsync(Guid userId);
        Task<SavedSearch?> GetByIdAndUserIdAsync(Guid id, Guid userId);
        Task IncrementUseCountAsync(Guid id);
    }
}

