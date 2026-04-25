# Beef Burger - Good Hamburger 🍔

A simple, clean, and maintainable fullstack system for managing orders at a hamburger shop called "Beef Burger". Built with **.NET 8** (Clean Architecture API) and **Blazor WebAssembly .NET 9** (Frontend).

---

## Getting Started (Execution Instructions)

### Prerequisites
- Docker & Docker Compose installed on your machine.
- Git.

### Running the Application

This project is fully containerized. You **do not** need to install .NET SDKs to run it, as the `docker-compose` will build the source code securely inside isolated containers.

1. Clone the repository:
```bash
git clone https://github.com/alysonsz/beef-burger.git
cd beef-burger
```

2. Start all services (SQL Server, Redis, API, and Frontend):
```bash
docker compose up -d --build
```

3. Access the applications:
- **Frontend WebApp**: [http://localhost:5102](http://localhost:5102)
- **Backend API (Swagger)**: [http://localhost:8080/swagger](http://localhost:8080/swagger)

*(Note: Database migrations and Redis initialization are automatically applied on API startup)*

---

## Architecture & Technology Stack

The solution is divided into a Monorepo containing two main projects, fully orchestrated via Docker Compose:

### 1. Back-end API (C# / .NET 8)
Built strictly following **Clean Architecture** principles to ensure the separation of concerns:
- **Domain**: Contains the core business rules (`Order`, `OrderItem`), Enums, and validation logic.
- **Application**: Contains the DTOs (Data Transfer Objects) and Services (`OrderService`, `MenuItemService`).
- **Infrastructure**: Data access layer using **Entity Framework Core 8.0**, connecting to a **SQL Server** database and a **Redis** instance for high-performance caching.
- **API**: ASP.NET Core Web API layer containing the Controllers, Swagger UI, and global configurations (like custom JSON Serialization and CORS).

### 2. Front-end Web (Blazor WebAssembly / .NET 9)
A SPA (Single Page Application) built with Blazor WebAssembly.
- **State Management**: Uses manual event callbacks and `StateHasChanged()` to ensure synchronous UI updates matching the Redis cache behavior.
- **Styling**: Uses purely modular Vanilla CSS (`variables.css`, `layout.css`, `components.css`) for maximum customization and to avoid the overhead of large CSS frameworks.
- **Hosting**: Deployed behind an **Nginx** reverse proxy using a multi-stage Dockerfile.

---

## Architectural Decisions

During the development of this project, several architectural choices were made focusing on maintainability and enterprise readiness:

- **Monorepo Approach:** Both the API and Frontend reside in the same repository. This makes it easier for reviewers and other developers to run the entire stack with a single `docker compose up` command.
- **Redis for Menu Caching:** The menu items change rarely but are fetched frequently. Implementing a distributed cache with Redis significantly reduces the load on the SQL database and improves API response times.
- **Local Context Builds over Remote Images:** To maintain 100% transparency for technical evaluations, the `docker-compose.yml` uses local `build: context` rather than pulling static images from Docker Hub. This guarantees that the code being reviewed is exactly the code being executed.
- **Separation of Concerns without MediatR:** While Clean Architecture often pairs with MediatR, direct Service Injection (`IOrderService`) was chosen to avoid over-engineering and keep the execution flow straightforward and easy to trace for this domain size.
- **Value Objects / Domain Entities:** Business validations (such as preventing duplicate items and ensuring categories match) are deeply embedded inside the Domain (`Order.cs`), rather than in the API Controllers.
- **CI/CD Pipeline:** A GitHub Actions workflow (`ci.yml`) was added to ensure continuous integration. Every push automatically restores dependencies, builds both projects, and runs the `xUnit` test suite.

---

## What Was Left Out (Out of Scope)

Given the focus on the core business logic (Orders and Menu Items CRUD), some standard enterprise features were deliberately left out to keep the project scoped and straightforward:

1. **Authentication and Authorization:**
   - There are no JWT tokens or Identity providers (like IdentityServer or Auth0). The API is completely public. In a real-world scenario, endpoints would be secured based on roles (e.g., `Admin`, `Customer`).
2. **FluentValidation:**
   - While the Domain executes critical validation, complex DTO validations (like preventing empty strings or ensuring maximum string lengths on incoming requests) were kept minimal. A library like FluentValidation would be added in a production environment.
3. **Automated Docker Hub Deployments (CD):**
   - The CI/CD pipeline currently focuses on building and testing (`Continuous Integration`). Automated pushes to a container registry and auto-deployments to a cloud provider (like AWS or Azure) were left out.
4. **Resilience Policies (Polly):**
   - While Entity Framework is configured with basic connection retries, advanced resilience patterns (like Circuit Breakers for the Redis cache) using Polly were omitted. 

---

## Features & Business Rules

### Menu Items
- **Sandwiches**: X Burger, X Egg, X Bacon, and Custom items.
- **Sides**: Fries and Custom items.
- **Drinks**: Soft drink and Custom items.

### Orders & Discounts
- Each order can contain only ONE item from each category.
- **Dynamic Discounts**:
  - Sandwich + Fries + Drink → 20% discount
  - Sandwich + Drink → 15% discount
  - Sandwich + Fries → 10% discount
