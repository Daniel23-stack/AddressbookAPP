# Address Book Application

A modern web application for managing contacts and addresses, built with ASP.NET Core MVC.

## Features

- User Authentication (Login/Register)
- Contact Management
  - Add, edit, and delete contacts
  - View contact details
  - Search and filter contacts
- API Client Management
  - Generate and manage API keys
  - Monitor API usage
- Import/Export Functionality
  - Import contacts from CSV files
  - Export contacts to CSV format
- Dashboard with usage statistics
- Responsive design with Bootstrap 5

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server (for database)
- Visual Studio 2022 or Visual Studio Code (recommended)

## Getting Started

1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```

2. Navigate to the project directory:
   ```bash
   cd AddressBookApp
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Update the connection string in `appsettings.json` to point to your SQL Server instance.

5. Run the application:
   ```bash
   dotnet run
   ```

6. Open your browser and navigate to:
   - http://localhost:5000 or
   - https://localhost:5001

## Project Structure

```
AddressBookApp/
├── AddressBookApp.API/           # Main API project
│   ├── Controllers/              # MVC Controllers
│   ├── Models/                   # View Models
│   ├── Services/                 # Business Logic
│   ├── Views/                    # Razor Views
│   └── wwwroot/                  # Static Files
├── AddressBookApp.Business/      # Business Logic Layer
├── AddressBookApp.Data/          # Data Access Layer
└── AddressBookApp.Domain/        # Domain Models
```

## Authentication

The application uses JWT (JSON Web Tokens) for authentication. Users can:
- Register with email, first name, last name, and password
- Login with email and password
- Logout from the application

## API Documentation

The application provides a RESTful API for contact management. API clients can:
- Create, read, update, and delete contacts
- Generate and manage API keys
- Monitor API usage

## Security Features

- Password hashing using secure algorithms
- JWT-based authentication
- API key management
- CORS policy configuration
- Input validation and sanitization

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue in the repository or contact the development team.

## Acknowledgments

- ASP.NET Core
- Bootstrap 5
- jQuery
- SQL Server
