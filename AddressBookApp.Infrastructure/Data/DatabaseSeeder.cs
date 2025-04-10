using AddressBookApp.Core.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookApp.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly AddressBookContext _context;

        public DatabaseSeeder(AddressBookContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Only seed if the database is empty
            if (!_context.Users.Any() && !_context.Contacts.Any())
            {
                // Seed users
                var users = await SeedUsersAsync();
                
                // Seed contacts
                await SeedContactsAsync(users.adminUser);
            }
        }

        private async Task<(User adminUser, User regularUser)> SeedUsersAsync()
        {
            CreatePasswordHash("Admin123!", out byte[] adminPasswordHash, out byte[] adminPasswordSalt);
            CreatePasswordHash("User123!", out byte[] userPasswordHash, out byte[] userPasswordSalt);

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = adminPasswordHash,
                PasswordSalt = adminPasswordSalt,
                CreatedAt = DateTime.UtcNow
            };

            var regularUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                FirstName = "Regular",
                LastName = "User",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddRangeAsync(adminUser, regularUser);
            await _context.SaveChangesAsync();

            return (adminUser, regularUser);
        }

        private async Task SeedContactsAsync(User createdBy)
        {
            var contacts = new[]
            {
                new Contact
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Company = "ABC Corporation",
                    Notes = "Important client",
                    IsClient = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Company = "XYZ Ltd",
                    Notes = "Potential client",
                    IsClient = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Company = "Johnson & Co",
                    Notes = "Long-term client",
                    IsClient = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _context.Contacts.AddRangeAsync(contacts);
            
            // Add addresses for the first contact
            var johnDoeAddresses = new[]
            {
                new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA",
                    Type = AddressType.Work,
                    ContactId = contacts[0].Id
                },
                new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "456 Park Ave",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10002",
                    Country = "USA",
                    Type = AddressType.Home,
                    ContactId = contacts[0].Id
                }
            };
            
            await _context.Addresses.AddRangeAsync(johnDoeAddresses);
            
            // Add phone numbers for contacts
            var phoneNumbers = new[]
            {
                new PhoneNumber
                {
                    Id = Guid.NewGuid(),
                    Number = "212-555-1234",
                    Type = PhoneType.Work,
                    ContactId = contacts[0].Id
                },
                new PhoneNumber
                {
                    Id = Guid.NewGuid(),
                    Number = "917-555-5678",
                    Type = PhoneType.Mobile,
                    ContactId = contacts[0].Id
                },
                new PhoneNumber
                {
                    Id = Guid.NewGuid(),
                    Number = "415-555-9876",
                    Type = PhoneType.Work,
                    ContactId = contacts[1].Id
                }
            };
            
            await _context.PhoneNumbers.AddRangeAsync(phoneNumbers);
            
            // Add email addresses for contacts
            var emailAddresses = new[]
            {
                new EmailAddress
                {
                    Id = Guid.NewGuid(),
                    Email = "john.doe@abccorp.com",
                    Type = EmailType.Work,
                    ContactId = contacts[0].Id
                },
                new EmailAddress
                {
                    Id = Guid.NewGuid(),
                    Email = "john.personal@example.com",
                    Type = EmailType.Personal,
                    ContactId = contacts[0].Id
                },
                new EmailAddress
                {
                    Id = Guid.NewGuid(),
                    Email = "jane.smith@xyzltd.com",
                    Type = EmailType.Work,
                    ContactId = contacts[1].Id
                },
                new EmailAddress
                {
                    Id = Guid.NewGuid(),
                    Email = "bob@johnson.com",
                    Type = EmailType.Personal,
                    ContactId = contacts[2].Id
                }
            };
            
            await _context.EmailAddresses.AddRangeAsync(emailAddresses);
            await _context.SaveChangesAsync();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
} 