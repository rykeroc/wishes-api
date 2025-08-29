# Wishes API

## Overview

This is the REST API that backs the [Wishes application](https://github.com/rykeroc/wishes). 

## Development Setup

1. Clone the repository
2. Start a Postgres database. This can be done easily with docker.
3. Insert the Postgres database connection string into `appsettings.Development.json` under `ConnectionStrings/WishesContext`.
4. 

## Tools

### Entity Framework

#### CLI Tool

Install CLI tool:

```bash
  dotnet tool install dotnet-ef -g
```

Create Migrations:

```bash
dotnet-ef migrations add <migration-name>
```



### Postgres

## Resources

| Name                                    | URL                                        |
|-----------------------------------------|--------------------------------------------|
| Entity Framework Postgres Configuration | https://www.npgsql.org/efcore/?tabs=aspnet |