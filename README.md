# Beef Burger - Good Hamburger

A simple, clean, and maintainable fullstack system for managing orders at a hamburger shop called "Beef Burger". Built with **.NET 8** (Clean Architecture API) and **Blazor WebAssembly .NET 9** (Frontend).

## Architecture

The solution is divided into two main projects:

1. **Back-end API** (`/GoodHamburger.Api` etc) - Built with .NET 8 following clean architecture principles (Domain, Application, Infrastructure, API).
2. **Front-end Web** (`/beef-burger front-end`) - Built with Blazor WebAssembly .NET 9, styled with plain CSS for maximum customization and performance.

### Technology Stack

- **.NET 8.0 / .NET 9.0** - Frameworks
- **ASP.NET Core Web API** - REST API backend
- **Blazor WebAssembly** - SPA Frontend
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Relational database
- **Redis** - Distributed caching for menu items
- **Docker & Docker Compose** - Containerization & Orchestration
- **Nginx** - Web server serving the frontend static files

---

## Getting Started

### Prerequisites
- Docker & Docker Compose installed on your machine.

### Running with Docker Compose (Recommended)

This project is fully containerized. You do not need to install .NET SDKs to run it. The `docker-compose.yml` uses pre-built images from Docker Hub or builds them locally.

1. Clone the repository:
```bash
git clone https://github.com/alysonsz/beef-burger.git
cd beef-burger
```

2. Start all services (SQL Server, Redis, API, and Frontend):
```bash
docker compose up -d
```

3. Access the applications:
- **Frontend WebApp**: [http://localhost:5102](http://localhost:5102)
- **Backend API (Swagger)**: [http://localhost:8080/swagger](http://localhost:8080/swagger)

*(Note: Database migrations are automatically applied on API startup)*

---

## Features

### 🍔 Menu Items
- **Sandwiches**: X Burger, X Egg, X Bacon, and Custom items.
- **Sides**: Fries and Custom items.
- **Drinks**: Soft drink and Custom items.

### 🛒 Business Rules & Orders
- Each order can contain only ONE item from each category.
- **Dynamic Discounts**:
  - Sandwich + Fries + Drink → 20% discount
  - Sandwich + Drink → 15% discount
  - Sandwich + Fries → 10% discount

### 💻 Frontend Functionality
- Full CRUD for orders and custom menu items.
- Dynamic caching invalidation.
- Responsive design and dynamic active-state routing.

---

## Design Principles

- **SOLID Principles** - Clear separation of concerns with layered architecture.
- **Container-Ready** - Multi-stage builds for both .NET API and Blazor Wasm.
- **Production-Ready** - Uses SQL Server and Redis for enterprise scenarios.
