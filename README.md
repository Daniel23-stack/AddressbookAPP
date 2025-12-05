# Address Book Application

A modern, feature-rich web application for managing contacts and addresses, built with ASP.NET Core MVC. This application provides a comprehensive solution for contact management with advanced features including groups, tags, search, analytics, and more.

## ✨ Features

### 🔐 Authentication & Security
- User registration and login with email/password
- Cookie-based authentication with JWT support
- Secure password hashing
- Two-Factor Authentication (2FA) support (infrastructure ready)
- User profile management

### 📇 Contact Management
- **Full CRUD Operations**: Create, read, update, and delete contacts
- **Multiple Contact Details**: 
  - Multiple addresses per contact
  - Multiple phone numbers per contact
  - Multiple email addresses per contact
- **Contact Organization**:
  - Contact Groups (custom groups like "Family", "Work", "Clients")
  - Contact Tags (flexible tagging system)
  - Many-to-many relationships
- **Contact Photos**: Upload and manage contact photos/avatars
- **Contact Notes**: Enhanced notes with types, categories, and reminders
- **Custom Fields**: User-defined fields for contacts
- **Activity History**: Complete audit trail of all contact changes

### 🔍 Search & Filtering
- **Basic Search**: Quick search across contact fields
- **Advanced Search**: 
  - Filter by date range, company, groups, tags
  - Search within notes
  - Multiple criteria combinations
- **Saved Searches (Smart Lists)**: Save and reuse complex search filters
- **Real-time Search**: Instant results as you type

### 📊 Dashboard & Analytics
- **Interactive Charts**: 
  - Contact growth over time
  - Company distribution
  - Activity timeline
  - API usage statistics
- **Statistics Cards**: Total contacts, clients, imported contacts
- **Recent Activity Feed**: Latest actions and changes
- **Customizable Widgets**: Drag-and-drop dashboard layout (planned)

### 🔌 API Management
- **API Client Management**: Generate and manage API keys
- **API Usage Tracking**: Monitor API calls and endpoints
- **RESTful API**: Full API for contact management
- **API Documentation**: Swagger/OpenAPI support

### 📥 Import/Export
- **CSV Import**: Import contacts from CSV files
- **CSV Export**: Export contacts to CSV format
- **Export History**: Track all export operations
- **Import History**: Track import operations with success/failure counts

### 🎨 User Interface
- **Modern Design**: Beautiful, responsive UI with Bootstrap 5
- **Card/List View Toggle**: Switch between card and list views
- **Pagination**: Efficient handling of large contact lists
- **Skeleton Loading States**: Visual feedback during data loading
- **Empty States**: User-friendly messages for empty data
- **Toast Notifications**: Non-intrusive success/error messages
- **Keyboard Shortcuts**: Power user productivity features
- **Context Menu**: Right-click quick actions
- **Drag & Drop**: Reorder contacts and groups
- **Responsive Design**: Works on desktop, tablet, and mobile

### 📱 Progressive Web App (PWA)
- **Offline Support**: Service worker for offline functionality
- **Installable**: Can be installed as a native app
- **Mobile-Optimized**: Optimized for mobile devices
- **Push Notifications**: Infrastructure ready for notifications

## 🏗️ Architecture

This application follows a **Clean Architecture** pattern with clear separation of concerns:

```
AddressBookApp/
├── AddressBookApp.API/              # Presentation Layer (MVC)
│   ├── Controllers/                 # MVC Controllers
│   ├── Models/                      # View Models
│   ├── Views/                       # Razor Views
│   └── wwwroot/                     # Static Files (CSS, JS, Images)
│
├── AddressBookApp.Application/      # Application Layer
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Mappings/                    # AutoMapper Profiles
│   └── Services/                    # Application Services
│
├── AddressBookApp.Core/              # Domain Layer
│   ├── Interfaces/                  # Service & Repository Interfaces
│   └── Models/                      # Domain Models/Entities
│
└── AddressBookApp.Infrastructure/   # Infrastructure Layer
    ├── Data/                        # DbContext & Database Configuration
    ├── Migrations/                  # Entity Framework Migrations
    └── Repositories/                # Repository Implementations
```

## 📋 Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server** (LocalDB, Express, or Full) or **SQL Server Express LocalDB**
- **Visual Studio 2022** or **Visual Studio Code** (recommended)
- **Git** (for cloning the repository)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone [repository-url]
cd AddressbookAPP
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Database Connection

Update the connection string in `AddressBookApp.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AddressBookDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 4. Run Database Migrations

```bash
dotnet ef database update --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

This will create the database and apply all migrations.

### 5. Trust Development Certificate (for HTTPS)

```bash
dotnet dev-certs https --trust
```

### 6. Run the Application

```bash
dotnet run --project AddressBookApp.API/AddressBookApp.API.csproj
```

Or from the API project directory:

```bash
cd AddressBookApp.API
dotnet run
```

### 7. Access the Application

Open your browser and navigate to:

- **HTTP**: `http://localhost:5003`
- **HTTPS**: `https://localhost:5004`

The application will automatically seed the database with sample data on first run.

## 🔧 Configuration

### Port Configuration

Default ports are configured in `appsettings.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5003"
      },
      "Https": {
        "Url": "https://localhost:5004"
      }
    }
  }
}
```

### JWT Configuration

JWT settings are configured in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKey...",
    "Issuer": "AddressBookApp",
    "Audience": "AddressBookApp"
  }
}
```

**⚠️ Important**: Change the JWT key in production!

## 📚 API Documentation

Once the application is running, you can access:

- **Swagger UI**: `https://localhost:5004/swagger` (Development only)
- **API Endpoints**: All API endpoints are prefixed with `/api/`

### Example API Endpoints

- `GET /api/contacts` - Get all contacts
- `POST /api/contacts` - Create a new contact
- `GET /api/contacts/{id}` - Get a specific contact
- `PUT /api/contacts/{id}` - Update a contact
- `DELETE /api/contacts/{id}` - Delete a contact
- `POST /api/contacts/search/advanced` - Advanced search

## 🗄️ Database Schema

### Main Tables

- **Users**: User accounts and authentication
- **Contacts**: Contact information
- **Addresses**: Contact addresses
- **PhoneNumbers**: Contact phone numbers
- **EmailAddresses**: Contact email addresses
- **ContactGroups**: User-defined contact groups
- **ContactTags**: User-defined contact tags
- **ContactActivities**: Activity history and audit trail
- **ContactPhotos**: Contact photos and avatars
- **ContactNotes**: Enhanced notes and reminders
- **CustomFieldDefinitions**: User-defined custom fields
- **CustomFieldValues**: Values for custom fields
- **SavedSearches**: Saved search filters (Smart Lists)
- **ApiClients**: API client management
- **ApiUsages**: API usage tracking
- **DataExports**: Export history
- **ImportHistories**: Import history

## 🎯 Key Features in Detail

### Contact Groups & Tags

Organize contacts into custom groups and add tags for flexible categorization:

- Create unlimited groups (e.g., "Family", "Work", "Clients")
- Assign multiple tags to contacts
- Filter contacts by group or tag
- Color-coded groups for visual organization

### Advanced Search

Powerful search capabilities:

- Search by name, company, email, phone
- Filter by date range, groups, tags
- Search within notes
- Save searches as "Smart Lists"
- Combine multiple criteria

### Dashboard Analytics

Comprehensive analytics and visualizations:

- Contact growth charts
- Company distribution
- Activity timeline
- API usage statistics
- Recent activity feed

### Activity History

Complete audit trail:

- Track all contact changes
- View field-level changes (old/new values)
- Monitor user actions
- Track IP addresses and user agents
- Activity types: Created, Updated, Deleted, Viewed, etc.

## 🔒 Security Features

- **Password Hashing**: Secure password storage using industry-standard algorithms
- **JWT Authentication**: Token-based authentication for API access
- **Cookie Authentication**: Secure cookie-based authentication for web UI
- **CORS Policy**: Configurable Cross-Origin Resource Sharing
- **Input Validation**: Server-side validation and sanitization
- **SQL Injection Protection**: Parameterized queries via Entity Framework
- **XSS Protection**: Built-in ASP.NET Core protections

## 🧪 Testing

### Running Tests

```bash
dotnet test
```

### Manual Testing

1. Register a new user account
2. Login with your credentials
3. Create contacts and test CRUD operations
4. Try advanced search and filtering
5. Create groups and tags
6. Test import/export functionality
7. Explore the dashboard analytics

## 📦 Dependencies

### Core Technologies

- **ASP.NET Core 8.0**: Web framework
- **Entity Framework Core**: ORM for database access
- **SQL Server**: Database
- **AutoMapper**: Object-to-object mapping
- **Bootstrap 5**: CSS framework
- **jQuery**: JavaScript library
- **Chart.js**: Chart visualization library

### NuGet Packages

- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `AutoMapper`
- `AutoMapper.Extensions.Microsoft.DependencyInjection`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Swashbuckle.AspNetCore` (Swagger)

## 🛠️ Development

### Adding a New Migration

```bash
dotnet ef migrations add MigrationName --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Applying Migrations

```bash
dotnet ef database update --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### Dropping Database

```bash
dotnet ef database drop --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API --force
```

## 📝 Project Structure Details

### Controllers

- `HomeController`: Landing page and contact views
- `AccountController`: Authentication (login, register, logout)
- `ContactsController`: Contact API endpoints
- `DashboardController`: Dashboard and analytics
- `SavedSearchesController`: Saved searches management
- `ApiClientsController`: API client management
- `ImportExportController`: Import/export functionality

### Services

- `ContactService`: Contact business logic
- `AuthService`: Authentication logic
- `DashboardService`: Analytics and statistics
- `SavedSearchService`: Saved search management
- `ApiClientService`: API client management
- `ImportExportService`: Import/export operations

### Models

All domain models are in `AddressBookApp.Core/Models/`:

- `Contact`, `Address`, `PhoneNumber`, `EmailAddress`
- `ContactGroup`, `ContactTag`
- `ContactActivity`, `ContactPhoto`, `ContactNote`
- `CustomFieldDefinition`, `CustomFieldValue`
- `SavedSearch`, `User`, `ApiClient`, etc.

## 🚧 Planned Features

- [ ] Two-Factor Authentication (2FA) UI
- [ ] Calendar Integration
- [ ] Social Media Integration
- [ ] Enhanced Export (PDF, Excel, vCard)
- [ ] Contact Duplicate Detection
- [ ] Bulk Operations
- [ ] Contact Relationships
- [ ] Email Integration
- [ ] Dark Mode
- [ ] Advanced Permissions & Roles

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support, please:
- Open an issue in the repository
- Check existing issues for solutions
- Contact the development team

## 🙏 Acknowledgments

- **ASP.NET Core** - Web framework
- **Entity Framework Core** - ORM
- **Bootstrap 5** - CSS framework
- **Chart.js** - Chart library
- **jQuery** - JavaScript library
- **SQL Server** - Database

## 📞 Contact

For questions or suggestions, please open an issue in the repository.

---

**Built with ❤️ using ASP.NET Core**
