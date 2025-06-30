# Document Access Approval System

A clean architecture-based web application built with ASP.NET Core and Entity Framework Core to manage document access requests, approvals with role-based access control, background processing.

---
## 🧱 Tech Stack

- ASP.NET Core 9

- EF Core 9 + SQLite or InMemory

- xUnit for UnitTesting

- Clean Architecture (Domain + Application + Infrastructure + API)

---

## 🧱 Project Structure & Key Design Decisions

This project follows the principles of Clean Architecture, structured into layered components that enforce separation of concerns, testability, and independent deployability. Each layer has a distinct responsibility and depends only on abstractions, not concrete implementations.

### 📐 SOLID Principles in Practice
This system also adheres to the SOLID principles of object-oriented design to ensure maintainable and scalable code:

**S — Single Responsibility Principle:**

Each class (e.g., command handler, service, validator) has one clearly defined job, such as handling a request or validating input.

**O — Open/Closed Principle:**

The application layer is designed to be open for extension but closed for modification. New features (e.g., a new approval policy) can be added without touching core logic.

**L — Liskov Substitution Principle:**

Interfaces (e.g., IRequestService, IDecisionService) are honored consistently across implementations, allowing for test stubs and mocks without breaking the contract.

**I — Interface Segregation Principle:**

Interfaces are minimal and focused. For example, IRequestService and IDecisionService do not force consuming classes to implement unused methods.

**D — Dependency Inversion Principle:**

Higher-level modules (like use cases) depend on abstractions (IRepository, IMediator) rather than EF Core or web frameworks directly.

--


### 🧱 Layered Breakdown

DocumentAccessApprovalSystem/

├── DocumentAccessApprovalSystem.Domain/    - Enterprise rules (Entities, Enums)

├── DocumentAccessApprovalSystem.Application/       - DTOs,Iinterfaces ,(pure logic)

├── DocumentAccessApprovalSystem.Infrastructure/    - EF Core, external service implementations (e.g., Email, DB)

├── DocumentAccessApprovalSystem.API/             - API Controllers, DI setup, configuration

├── DocumentAccessApprovalSystem.Tests/             - Unit and integration test projects


### Key Decisions

- **FluentValidation** for request validation.
- **Entity Framework Core** with code-first migrations.
- **Dependency Injection** for all services.
- **Clean separation of concerns** to facilitate testing and future extensibility.

---

## ✅ Features Implemented

### 🔐 Authentication & Role Separation

- JWT-based authentication

- Roles: User (requests access) and Approver (reviews/decides)

- Secure endpoints via [Authorize(Roles = "User")] or [Authorize(Roles = "Approver")]

### 📑 Swagger Documentation

- API fully documented via Swagger UI

- JWT bearer auth integrated in Swagger

- Descriptive endpoint summaries and responses

### 🔄 CQRS (Command Query Responsibility Segregation)

#### Commands:

- CreateAccessRequest

- MakeDecision

#### Queries:

- GetUserRequestsQuery

- GetPendingRequestsQuery

#### Handlers inject DbContext and return DTOs or domain entities

#### ⚙️ Background Jobs / Event Simulation

- Simulated using IHostedService

- NotificationWorker logs request status updates

- EmailWorker logs simulated email delivery

- Messages are queued via INotificationQueue and IEmailQueue

#### 📁 File-Based Logging + REST API

- Logs written to:

    - Logs/notifications.log

    - Logs/emails.log

- View logs via:

     -GET /api/logs/notifications

     -GET /api/logs/emails

#### 🧪 Unit Test Example

- CreateAccessRequestHandlerTests.cs

- Uses EF Core InMemory

- Validates request is persisted with correct values


----

## 🧠 Assumptions & Trade-offs

- Assumes a single-level approval workflow per document request.
- Role-based access is basic; no advanced RBAC or OAuth2 support included yet.
- Persistence uses a local SQLite database via Entity Framework Core — ideal for quick development and testing, but may require migration to SQL Server or PostgreSQL for production scenarios.
- Focused on backend logic; frontend is not included.



Trade-offs:
- Clean Architecture for maintainability over simplicity.
- Chose a synchronous approval process to simplify logic—may not scale for async workflows.

---

## 🚀 Running the App

1. **Set up the database (SQLite)**

    No separate setup is needed — the SQLite database file will be created automatically when the application runs.

    ✅ Make sure your appsettings.json or appsettings.Development.json has a valid SQLite connection string, e.g.:

    - Example :  **json**
  
        "ConnectionStrings": 

        {
            
        "DefaultConnection": "Data Source=app.db"

        }

2. **Run the application**

    - Clone repo

    - Restore packages : 
        
            dotnet restore

    - Run 

            cd source\repos\DocumentAccessApprovalSystem\DocumentAccessApprovalSystem.API

            dotnet run

    - Browse to Swagger UI : 

            https://localhost:<port>/swagger

    - Run tests : 

            dotnet test DocumentAccessApprovalSystem.Tests

3. **Apply EF Core migrations (if needed)**

    If migrations are not already applied or included:

    💡 Ensure you have the dotnet-ef tool installed:

        dotnet tool install --global dotnet-ef
    Switch to Directory where DbContext is present in .csproj

            cd source\repos\DocumentAccessApprovalSystem\DocumentAccessApprovalSystem.Infrastructure

            dotnet ef migrations add Init

            dotnet ef database update
    ---


## 👥 Users

Varun  —  role: User

Vidya — role: Approver

    Use /api/auth/login to simulate login and receive JWTs.
    
---

## 🔚 Conclusion

This implementation focuses on clarity, clean layering, and demonstrated simulation of production-like behavior — ideal for team handoffs.


