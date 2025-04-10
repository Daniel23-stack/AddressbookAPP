using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
