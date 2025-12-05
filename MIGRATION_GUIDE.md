# Database Migration Guide

## How to Create a New Migration

### Step 1: Make Changes to Your Models
First, modify your models in `AddressBookApp.Core/Models/` or update `AddressBookContext.cs` in `AddressBookApp.Infrastructure/Data/`

### Step 2: Create the Migration
Run this command from the **project root** directory:

```powershell
dotnet ef migrations add YourMigrationName --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

**Example:**
```powershell
dotnet ef migrations add AddContactGroups --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Step 3: Review the Migration
The migration file will be created in `AddressBookApp.Infrastructure/Migrations/`
- Review the `Up()` method (applies changes)
- Review the `Down()` method (rolls back changes)

### Step 4: Apply the Migration
```powershell
dotnet ef database update --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

## Common Migration Commands

### List All Migrations
```powershell
dotnet ef migrations list --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Apply Specific Migration
```powershell
dotnet ef database update MigrationName --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Rollback Last Migration
```powershell
dotnet ef database update PreviousMigrationName --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Remove Last Migration (before applying)
```powershell
dotnet ef migrations remove --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

## Example: Adding Contact Groups Feature

### 1. Create the Model
```csharp
// AddressBookApp.Core/Models/ContactGroup.cs
public class ContactGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Contact> Contacts { get; set; }
}
```

### 2. Update DbContext
```csharp
// AddressBookApp.Infrastructure/Data/AddressBookContext.cs
public DbSet<ContactGroup>? ContactGroups { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // ... existing code ...
    
    modelBuilder.Entity<ContactGroup>()
        .HasMany(g => g.Contacts)
        .WithMany(c => c.Groups)
        .UsingEntity(j => j.ToTable("ContactGroupContacts"));
}
```

### 3. Create Migration
```powershell
dotnet ef migrations add AddContactGroups --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### 4. Apply Migration
```powershell
dotnet ef database update --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

## Troubleshooting

### Error: "No DbContext was found"
- Make sure you're running from the project root
- Check that `AddressBookContext` is in the correct namespace
- Verify the startup project path is correct

### Error: "Unable to create an object of type 'AddressBookContext'"
- Check your `appsettings.json` connection string
- Ensure SQL Server is running
- Verify connection string format

### Migration Already Exists
- Remove the last migration: `dotnet ef migrations remove`
- Or create with a different name

## Best Practices

1. **Name migrations descriptively**: `AddContactGroups`, `UpdateUserTable`, etc.
2. **Review generated code** before applying
3. **Test migrations** on a development database first
4. **Backup database** before applying to production
5. **One feature per migration** when possible
6. **Keep migrations small** and focused

## Migration File Structure

```csharp
public partial class YourMigrationName : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Changes to apply
        migrationBuilder.CreateTable(...);
        migrationBuilder.AddColumn(...);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // How to rollback
        migrationBuilder.DropTable(...);
        migrationBuilder.DropColumn(...);
    }
}
```

