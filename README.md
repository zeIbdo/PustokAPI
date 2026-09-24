# PustokAPI

A RESTful e-commerce backend developed with **ASP.NET Core 8**, following a layered architecture and providing authentication, product management, basket, and order functionality.

## Technologies

* **C# / .NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT Authentication**
* **FluentValidation**
* **Swagger / OpenAPI**

## Features

* User registration and authentication
* JWT-based authentication and authorization
* Role-based authorization
* Product, category, and tag management
* Basket management
* Order management
* Product image upload and management
* DTO-based API responses
* Request validation
* Pagination and filtering
* Entity Framework Core migrations and relationships

## Architecture

The application is organized into separate layers:

```text
PustokAPI
├── Pustok.API
├── Pustok.Application
├── Pustok.Domain
└── Pustok.Infrastructure
```

* **API** — Controllers, authentication configuration, and API endpoints
* **Application** — DTOs, services, business logic, and validation
* **Domain** — Entities and core domain models
* **Infrastructure** — Database access, repositories, Identity, and external services

## Authentication & Authorization

The API uses **ASP.NET Core Identity** for user management and **JWT Bearer Authentication** for securing API endpoints.

Role-based authorization is used to restrict access to administrative functionality.

## Database

**SQL Server** is used as the relational database, with **Entity Framework Core** handling:

* Entity relationships
* Database migrations
* Data access
* LINQ queries
* Tracking and no-tracking queries

## API Documentation

Swagger/OpenAPI is included to provide interactive API documentation and endpoint testing.

## Repository

GitHub: https://github.com/zeIbdo/PustokAPI
