# Maintenance Tracker MVP  
*A Blazor Server + ASP.NET Core C# project built for learning and demonstration purposes.*

---

## 🎯 Purpose
This MVP project was created to demonstrate **full-stack C# development** aligned with Detroit Defense’s technology stack.  
It implements a simple maintenance-tracking system for assets (e.g., vehicles or generators) using **Blazor Server**, **.NET 8**, and **Entity Framework Core (SQLite)**.

---

## 🧩 Tech Stack
| Layer | Technology |
|-------|-------------|
| Front-End | Blazor Server (.NET 8) |
| Back-End | ASP.NET Core Minimal APIs |
| Data Layer | Entity Framework Core 8 + SQLite |
| Language | C# |
| Testing | xUnit (unit tests) |
| Version Control | Git / GitHub |

---

## 🧱 Architecture Overview
Clean Architecture–inspired separation of concerns:
Domain/ → Core business models & rules
Data/ → EF Core DbContext + config
Components/ → Razor UI (pages + layouts)
Program.cs → App startup + Minimal APIs

---

## ⚙️ Key Features

### 1️⃣ C# / .NET Core Usage
- **Files:** `Domain/Entities.cs`, `Data/AppDbContext.cs`, `Program.cs`  
- Implements `Asset` and `WorkOrder` classes, EF Core mappings, and a SQLite database.  
- Follows clean separation between **Domain**, **Data**, and **UI** layers.

### 2️⃣ Blazor Front-End
- **Files:** `Components/Pages/Index.razor`, `Components/Pages/WorkOrders.razor`, `Components/Layout/NavMenu.razor`  
- Uses `<EditForm>` with data binding (`@bind-Value`) and validation (`DataAnnotationsValidator`).  
- Demonstrates routing (`@page "/workorders"`) and interactive server rendering (`@rendermode InteractiveServer`).  
- Real-time UI updates after database changes.

### 3️⃣ REST APIs (Minimal APIs)
- **File:** `Program.cs`  
- Endpoints:
  - `GET /api/workorders` → List work orders  
  - `POST /api/workorders` → Create new work order  
  - `PUT /api/workorders/{id}/status` → Forward-only status transitions  
- Demonstrates antiforgery handling, DI, and JSON serialization.

### 4️⃣ Testing Mindset
- **File:** `MaintenanceTracker.Mvp.Tests/StatusTests.cs` *(planned / optional)*  
- Validates that work-order status changes follow defined C# rules.  
- Uses xUnit + FluentAssertions-style checks.

### 5️⃣ Version Control & Collaboration
- **Files:** `.gitignore`, `.gitattributes`, commit history  
- Managed via GitHub to mirror professional CI/CD workflows.  
- Each feature committed separately to reflect real ticket-based development.

---

## 🚀 Running the App


# Run Blazor Server App
Open up MaintenanceTracker.Mvp snl in folder then build then debug with F5.

# Access in browser
https://localhost:####/

Default pages:

Home → dashboard KPIs

Work Orders → add new orders and advance statuses

API JSON endpoint → /api/workorders
