# Wishes API

## Overview

This is the REST API that backs the [Wishes application](https://github.com/rykeroc/wishes). 

## Development Setup

1. Clone the repository
2. Start a Postgres database. This can be done easily with docker.
3. Insert the Postgres database connection string into `appsettings.Development.json` under `ConnectionStrings/WishesContext`.
4. Apply [database migrations](#drop-database).
5. Set the user secrets `Authentication:Google:ClientSecret` and `Authentication:Google:ClientId` to the values found in the GCP console. [See here](#secret-management) for info on secrets storage.
6. Start the application using:

```bash
dotnet run
```

## Tools

### Entity Framework

#### CLI Tool

The `dotnet-ef` CLI tool can be installed and used to perform operations on the database for this project.

##### Install CLI tool

```bash
  dotnet tool install dotnet-ef -g
```

##### Create Migrations

```bash
dotnet-ef migrations add <migration-name> -o <output-dir>
```

##### Apply Migrations

```bash
dotnet-ef database update
```

##### Drop database

```bash
dotnet-ef database drop
```

### Secret Management

The Secret Manager tool is designed specifically for storing sensitive data during application development.
It helps manage app secrets by storing them in a location separate from the project tree, ensuring they are not 
checked into source control.

The user secrets configuration source is automatically registered in Development mode when 
`WebApplication.CreateBuilder`or `CreateDefaultBuilder` is used.
If `CreateDefaultBuilder` is not used, `builder.AddUserSecrets<Program>()` must be explicitly called in 
`ConfigureAppConfiguration`, but only in the Development environment.

Secrets can be read using the .NET Configuration API via constructor injection of `IConfiguration` 
(e.g., _config["Movies:ServiceApiKey"]).

#### Usage

##### Initialization

```bash
dotnet user-secrets init
```

##### Setting a secret

```bash
dotnet user-secrets set "Movies:ServiceApiKey" "12345"
```

NOTE: A colon nests a secret key into a JSON object.

### Postgres

## Resources

| Name                                    | URL                                        |
|-----------------------------------------|--------------------------------------------|
| Entity Framework Postgres Configuration | https://www.npgsql.org/efcore/?tabs=aspnet |