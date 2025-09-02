# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

WishesAPI is a .NET 9.0 REST API that serves as the backend for a wishlist management application. It uses Entity Framework Core with PostgreSQL and provides authentication and wishlist management capabilities.

## Development Commands

### Building and Running

```bash
# Build the solution
dotnet build

# Run the API in development mode
dotnet run --project WishesAPI/WishesAPI.csproj

# Run with specific launch profile
dotnet run --project WishesAPI/WishesAPI.csproj --launch-profile https

# Watch mode for development (auto-restart on file changes)
dotnet watch --project WishesAPI/WishesAPI.csproj
```

### Testing

```bash
# Run all tests (when test projects are added)
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Management (Entity Framework)

```bash
# Install EF CLI tools (one-time setup)
dotnet tool install dotnet-ef -g

# Add migration
dotnet ef migrations add <MigrationName> --project WishesAPI/WishesAPI.csproj

# Update database
dotnet ef database update --project WishesAPI/WishesAPI.csproj

# Drop database
dotnet ef database drop --project WishesAPI/WishesAPI.csproj

# Generate SQL script from migrations
dotnet ef migrations script --project WishesAPI/WishesAPI.csproj
```

### Docker Operations

```bash
# Build and run with Docker Compose
docker compose up --build

# Run in detached mode
docker compose up -d

# Stop services
docker compose down

# Build just the API image
docker build -f WishesAPI/Dockerfile -t wishesapi .
```

## Architecture & Structure

### Core Components

- **Program.cs**: Application entry point with minimal API setup, DbContext configuration, and OpenAPI integration
- **Data/WishesContext.cs**: Entity Framework DbContext managing all database entities
- **Models/**: Domain entities representing the core business objects
- **Controllers/**: API controllers (currently only AuthController with placeholder endpoints)

### Data Model

The application follows a user-centric wishlist model:

- **User**: Central entity representing application users
- **Account**: OAuth provider accounts linked to users (Google, etc.)
- **Wishlist**: User-owned collections of wishes with metadata (name, description, emoji, privacy settings)
- **Wish**: Individual items within wishlists with pricing, images, and priority

### Key Relationships

- User → Accounts (1:Many) - For OAuth provider integration
- User → Wishlists (1:Many) - Users can have multiple wishlists
- Wishlist → Wishes (1:Many) - Each wishlist contains multiple wishes

### Configuration

- **Development**: Uses `appsettings.Development.json` for local PostgreSQL connection
- **Production**: Environment variables and `appsettings.json` for production settings
- **Database**: PostgreSQL with connection string stored in `ConnectionStrings:WishesContext`

### Development Environment

- **Ports**: HTTP on 5234, HTTPS on 7183 (configured in launchSettings.json)
- **Database**: PostgreSQL (must be running locally or via Docker)
- **Authentication**: OAuth-based (Google) - currently stubbed in AuthController

## Important Notes

- The project requires a PostgreSQL database connection string in `appsettings.Development.json`
- Authentication endpoints in AuthController are currently placeholder implementations (marked with TODO comments)
- The application uses Entity Framework Core with connection pooling for better performance
- Docker support is configured but requires database connectivity
- The codebase is set up for .NET 9.0 with nullable reference types enabled
