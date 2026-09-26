# Gamana Muuttopalvelu - Backend API

The RESTful API backend for **Gamana Muuttopalvelu** (`gamanamuutto.fi`), built with .NET Core. Handles moving distance pricing calculations, booking persistence, and communication services.

---

## 🛠️ Tech Stack

* **Framework:** .NET Core Web API
* **Database & ORM:** PostgreSQL / Entity Framework Core
* **Email Service:** Resend
* **Hosting:** Render Web Service

---

## ⚡ Local Development Setup

### Prerequisites
* **.NET SDK**: `v8.0+`
* Running database instance (PostgreSQL)

### Installation & Run

1. Configure connection strings in `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=GamanaDb;Username=postgres;Password=yourpassword"
     }
   }
   ```

2. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

3. Launch the API server:
   ```bash
   dotnet run
   ```

---

## 🌐 CORS & Security Setup

Ensure cross-origin requests from the production Angular application are permitted in `Program.cs`:

```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy => {
        policy.WithOrigins("https://gamanamuutto.fi", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```
