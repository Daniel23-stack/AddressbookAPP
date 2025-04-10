using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, User User)> LoginAsync(string email, string password);
        Task<(bool Success, string Message)> RegisterAsync(string email, string password, string firstName, string lastName);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
