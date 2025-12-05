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
        public DbSet<ContactGroup>? ContactGroups { get; set; }
        public DbSet<ContactTag>? ContactTags { get; set; }
        public DbSet<SavedSearch>? SavedSearches { get; set; }
        public DbSet<ContactActivity>? ContactActivities { get; set; }
        public DbSet<ContactPhoto>? ContactPhotos { get; set; }
        public DbSet<ContactNote>? ContactNotes { get; set; }
        public DbSet<CustomFieldDefinition>? CustomFieldDefinitions { get; set; }
        public DbSet<CustomFieldValue>? CustomFieldValues { get; set; }

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

            // Configure ContactGroup entity
            modelBuilder.Entity<ContactGroup>()
                .HasIndex(g => new { g.Name, g.UserId })
                .IsUnique();

            modelBuilder.Entity<ContactGroup>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ContactTag entity
            modelBuilder.Entity<ContactTag>()
                .HasIndex(t => new { t.Name, t.UserId })
                .IsUnique();

            modelBuilder.Entity<ContactTag>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many: Contact <-> ContactGroup
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Groups)
                .WithMany(g => g.Contacts)
                .UsingEntity(j => j.ToTable("ContactGroupContacts"));

            // Configure many-to-many: Contact <-> ContactTag
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Tags)
                .WithMany(t => t.Contacts)
                .UsingEntity(j => j.ToTable("ContactTagContacts"));

            // Configure SavedSearch entity
            modelBuilder.Entity<SavedSearch>()
                .HasIndex(s => new { s.Name, s.UserId })
                .IsUnique();

            modelBuilder.Entity<SavedSearch>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ContactActivity entity
            modelBuilder.Entity<ContactActivity>()
                .HasOne(a => a.Contact)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContactActivity>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ContactActivity>()
                .HasIndex(a => new { a.ContactId, a.Timestamp });

            // Configure ContactPhoto entity
            modelBuilder.Entity<ContactPhoto>()
                .HasOne(p => p.Contact)
                .WithMany(c => c.Photos)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContactPhoto>()
                .HasOne(p => p.UploadedBy)
                .WithMany()
                .HasForeignKey(p => p.UploadedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure ContactNote entity
            modelBuilder.Entity<ContactNote>()
                .HasOne(n => n.Contact)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContactNote>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContactNote>()
                .HasIndex(n => new { n.ContactId, n.CreatedAt });

            // Configure CustomFieldDefinition entity
            modelBuilder.Entity<CustomFieldDefinition>()
                .HasIndex(f => new { f.Name, f.UserId })
                .IsUnique();

            modelBuilder.Entity<CustomFieldDefinition>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure CustomFieldValue entity
            modelBuilder.Entity<CustomFieldValue>()
                .HasOne(v => v.Contact)
                .WithMany(c => c.CustomFieldValues)
                .HasForeignKey(v => v.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomFieldValue>()
                .HasOne(v => v.FieldDefinition)
                .WithMany(f => f.Values)
                .HasForeignKey(v => v.FieldDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomFieldValue>()
                .HasIndex(v => new { v.ContactId, v.FieldDefinitionId })
                .IsUnique();
        }
    }
}
