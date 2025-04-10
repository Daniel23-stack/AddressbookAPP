using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AddressBookApp.Infrastructure
{
    class SeedData
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Address Book Database Seeder");
            Console.WriteLine("=============================");

            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Using connection string: {connectionString}");

            try
            {
                // Set up DbContext
                var options = new DbContextOptionsBuilder<AddressBookContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                // Create and run seeder
                using (var context = new AddressBookContext(options))
                {
                    var seeder = new DatabaseSeeder(context);
                    await seeder.SeedAsync();
                }
                
                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
} 