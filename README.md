# Good Hamburger Orders API

A simple, clean, and maintainable REST API for managing orders at a hamburger shop called "Good Hamburger". Built with .NET 8 following clean architecture principles.

## Architecture

The solution follows a layered architecture with clear separation of concerns:

- **GoodHamburger.Api** - Web API layer (controllers, HTTP handling)
- **GoodHamburger.Application** - Application services (use cases, DTOs)
- **GoodHamburger.Domain** - Domain entities and business rules
- **GoodHamburger.Infrastructure** - Data access (Entity Framework Core)
- **GoodHamburger.Tests** - Unit and integration tests

## Technology Stack

- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - Web framework
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Relational database (production)
- **Redis** - Distributed caching
- **Docker & Docker Compose** - Containerization
- **xUnit** - Testing framework
- **Swagger/OpenAPI** - API documentation

## Menu Items

### Sandwiches
- X Burger — $5.00
- X Egg — $4.50
- X Bacon — $7.00

### Sides
- Fries — $2.00

### Drinks
- Soft drink — $2.50

## Business Rules

### Order Validation
- Each order can contain only ONE sandwich
- Each order can contain only ONE fries
- Each order can contain only ONE drink
- Duplicate items will return a validation error

### Discount Rules
- Sandwich + fries + drink → 20% discount
- Sandwich + drink → 15% discount
- Sandwich + fries → 10% discount
- Otherwise → no discount

## API Endpoints

### Menu
- `GET /api/menu` - Returns available menu items

### Orders
- `POST /api/orders` - Create a new order
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `PUT /api/orders/{id}` - Update an order
- `DELETE /api/orders/{id}` - Delete an order

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Docker & Docker Compose (for containerized deployment)
- SQL Server (for local development without Docker)

### Option 1: Docker Compose (Recommended)

This is the easiest way to run the complete stack with SQL Server and Redis.

1. Copy the example environment file and customize if needed:
```bash
cp .env.example .env
```

The `.env` file contains default values that work out of the box for local development.

2. Build and start all services:
```bash
docker-compose up --build
```

The API will be available at `http://localhost:8080`
- SQL Server: `localhost:1433`
- Redis: `localhost:6379`

3. Apply database migrations (inside the container):
```bash
docker-compose exec api dotnet ef database update --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
```

4. Stop services:
```bash
docker-compose down
```

### Option 2: Local Development

1. Install SQL Server and Redis locally, then update `appsettings.json` with your connection strings.

2. Restore dependencies:
```bash
dotnet restore
```

3. Create and apply database migration:
```bash
dotnet ef migrations add InitialCreate --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
dotnet ef database update --project GoodHamburger.Infrastructure --startup-project GoodHamburger.Api
```

4. Run the API:
```bash
dotnet run --project GoodHamburger.Api
```

The API will be available at `http://localhost:5271`

### Swagger UI
In development mode, Swagger UI is available at:
```
http://localhost:8080/swagger (Docker)
http://localhost:5271/swagger (Local)
```

## Running Tests

Run the unit tests:
```bash
dotnet test
```

## Example Requests

### Create an order (sandwich + fries + drink = 20% discount)
```bash
POST /api/orders
Content-Type: application/json

{
  "sandwich": 500,
  "fries": 200,
  "drink": 250
}
```

Response:
```json
{
  "id": "guid",
  "sandwich": { "name": "X Burger", "price": 5.0 },
  "fries": { "name": "Fries", "price": 2.0 },
  "drink": { "name": "Soft drink", "price": 2.5 },
  "subtotal": 9.5,
  "discount": 1.9,
  "total": 7.6,
  "createdAt": "2024-04-24T13:31:33Z"
}
```

### Create an order (sandwich only = no discount)
```bash
POST /api/orders
Content-Type: application/json

{
  "sandwich": 500
}
```

## Technology Stack

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- SQL Server (relational database)
- Redis (distributed caching)
- Docker & Docker Compose
- xUnit for testing
- Swagger/OpenAPI for API documentation

## Design Principles

- **SOLID Principles** - Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Clean Architecture** - Clear separation of concerns with layered architecture
- **Simple & Explicit** - No over-engineering, no unnecessary abstractions
- **Services + Repositories** - Straightforward data access pattern
- **No Unit of Work** - Simple repository pattern without UoW
- **No MediatR** - Direct service calls without behavior/pipeline patterns
- **No Custom Middleware** - Uses standard ASP.NET Core middleware
- **Container-Ready** - Docker support for easy deployment
- **Production-Ready** - Uses SQL Server and Redis for enterprise scenarios

## License

This is a demonstration project for educational purposes.
