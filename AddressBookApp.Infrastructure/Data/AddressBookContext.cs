﻿using AddressBookApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Infrastructure.Data
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
        }

        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<Address>? Addresses { get; set; }
        public DbSet<PhoneNumber>? PhoneNumbers { get; set; }
        public DbSet<EmailAddress>? EmailAddresses { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<ApiClient>? ApiClients { get; set; }
        public DbSet<ApiUsage>? ApiUsages { get; set; }
        public DbSet<DataExport>? DataExports { get; set; }
        public DbSet<ImportHistory>? ImportHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Contact entity
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Contact)
                .HasForeignKey(a => a.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.PhoneNumbers)
                .WithOne(p => p.Contact)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.EmailAddresses)
                .WithOne(e => e.Contact)
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ApiClient entity
            modelBuilder.Entity<ApiClient>()
                .HasMany(c => c.ApiUsages)
                .WithOne(u => u.ApiClient)
                .HasForeignKey(u => u.ApiClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure ApiClient entity
            modelBuilder.Entity<ApiClient>()
                .HasIndex(c => c.ApiKey)
                .IsUnique();
        }
    }
}
