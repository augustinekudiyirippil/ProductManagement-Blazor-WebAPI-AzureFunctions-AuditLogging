Product Management Application with Azure Function Audit Logging
Overview

A cloud-based Product Management solution built using ASP.NET Core Web API, Blazor, Azure SQL Database, and Azure Functions. The application provides secure product management capabilities with JWT authentication and maintains a complete audit trail of all product operations through a dedicated Azure Function.
Features

    Product CRUD (Create, Read, Update, Delete) operations
    Blazor-based user interface
    ASP.NET Core Web API backend
    JWT Authentication and Authorization
    Azure SQL Database integration
    Azure App Service deployment
    Azure Function-based audit logging
    Entity Framework Core data access
    Dependency Injection and Repository Pattern
    Service Layer architecture

Technologies Used
Frontend

    Blazor

Backend

    ASP.NET Core Web API
    Entity Framework Core
    JWT Authentication

Database

    Azure SQL Database

Azure Services

    Azure App Service (API)
    Azure App Service (Blazor)
    Azure Functions
    Azure Storage Account
    Application Insights
    Azure Resource Group

Audit Logging Solution

A dedicated Azure Function (ProductAudit) captures all product activities and stores them in a ProductLog table.
Logged Events

    Product Created
    Product Updated
    Product Deleted

Logged Information

    Product ID
    User ID
    Action Type (Add / Edit / Delete)
    Action Date & Time
    Description

Authentication

The application uses JWT-based authentication.


Design Patterns

    Repository Pattern
    Service Layer Pattern
    Dependency Injection
    DTO Pattern
    Azure Function Integration Pattern

Azure Resources

    Resource Group
    API App Service
    Blazor App Service
    Azure SQL Server
    Azure SQL Database
    Azure Function App
    Azure Storage Account
    Application Insights

Learning Outcomes

This project demonstrates practical implementation of:

    ASP.NET Core API Development
    Blazor Development
    JWT Security
    Azure App Services
    Azure Functions
    Azure SQL Database
    Entity Framework Core
    REST API Design
    Cloud-Native Application Development
    Audit Logging Architecture
    Azure Resource Management
