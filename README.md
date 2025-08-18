# Simple Task Management System

This is my application **Task Management System**, built with **.NET 8**, **Blazor WebAssembly**, and the **Tailwind CSS** UI framework.

The application is designed to be easily extended and scalable. By default, it uses **SQLite** for simplicity, but you can switch to another database (e.g., SQL Server) by following the instructions below.

Unit tests are implemented using the **NUnit** framework.
The app uses **SignalR** to broadcast task changes (create/update/delete) to all connected clients in real time.
On startup, the API seeds the Tasks table using a **TaskSeeder**.

## 📂 Solution Structure

The solution contains 4 projects:

- **TaskManagementSystem.Api**: REST API project, handling business logic, data access, and migrations.

- **TaskManagementSystem.Client**: Blazor WebAssembly frontend, styled with Tailwind CSS.

- **TaskManagementSystem.Shared**: Contains shared DTOs and models used by both client and API.

- **TaskManagementSystem.Api.Tests**: Unit tests for the API layer using NUnit.

## 🚀 Getting Started

### Clone the repository

```bash
git clone https://github.com/bastienhatinguais/TaskManagementSystem.git
cd TaskManagementSystem
dotnet restore
```

### Database setup

Run migrations:

```bash
dotnet ef database update --project .\TaskManagementSystem.Api
```

By default, the app uses **SQLite**.
To switch to **SQL Server**, follow the instructions in the [Database Configuration](#-database-configuration) section.

## ▶️ Running the Application

### Start the API

```bash
dotnet run --project TaskManagementSystem.Api --launch-profile https
```

The API will be available at:
👉 [https://localhost:7069/api](https://localhost:7069/api)

Swagger documentation:
👉 [https://localhost:7069/swagger/index.html](https://localhost:7069/swagger/index.html)

### Start the Client

```bash
dotnet run --project TaskManagementSystem.Client --launch-profile https
```

The client will be available at:
👉 [https://localhost:7201/](https://localhost:7201/)

## 🎨 Client Development (Tailwind)

When working on the **client project**, you may need to regenerate Tailwind classes to reflect style updates.
Run the following commands in a separate terminal:

```bash
cd TaskManagementSystem.Client
npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css --watch
```

This will watch for changes in your CSS and rebuild `tailwind.css` automatically.

## 🧪 Running Tests

```bash
dotnet test
```

## ⚙️ Database Configuration

- **Default**: SQLite.
- **Switching to SQL Server**:
  1. Update the connection string in `appsettings.json` for the **API** project (and client, if applicable).
  2. In **TaskManagementSystem.Api/Program.cs**, update the `AddDbContext` configuration to use SQL Server instead of SQLite:
     ```csharp
     builder.Services.AddDbContext<AppDbContext>((sp, options) =>
     {
         var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

         // Change to select your database provider
         options.UseSqlServer(connectionString);
         //options.UseSqlite(connectionString);
     });
     ```
  3. Remove the existing `Migrations` folder in the **TaskManagementSystem.Api** project.
  4. Recreate the initial migration:
     ```bash
     dotnet ef migrations add InitialCreate --project .\TaskManagementSystem.Api
     ```
  5. Update the database:
     ```bash
     dotnet ef database update --project .\TaskManagementSystem.Api
     ```

## 💡 Notes

This project was built with **scalability and extensibility** in mind.
While some implementations may feel like "overkill" for a simple task management system, the intention was to design it so the project could easily be expanded in the future.
