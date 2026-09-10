# RaceDay API

**Event management platform for South African road running, walking and cycling events.**

A RESTful ASP.NET Core Web API backed by SQL Server via EF Core, with session-based authentication, role-based access control, Swagger documentation, unit tests and a GitHub Actions CI/CD pipeline.

> **Module:** PROG6212 — Programming 2B
> **Student:** A. Manqana (ST10470949)
> **Part:** 2 of 3 — RESTful API Development

##  Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Database Schema](#-database-schema)
- [Getting Started](#-getting-started)
- [Roles & Access Control](#-roles--access-control)
- [API Reference](#-api-reference)
- [Testing](#-testing)
- [CI/CD](#-cicd)
- [Video Walkthrough](#-video-walkthrough)
- [Roadmap](#-roadmap)
- [Author](#-author)

##  Overview

RaceDay is a Portfolio of Evidence project split into three parts:

| Part | Deliverable | Status |
|---|---|---|
| 1 | Planning — ERD, endpoint plan, SQL script | ✅ Complete |
| **2** | **RESTful API — this repository** | ✅ Complete |
| 3 | MVC front-end consuming this API | 🔜 Upcoming |

There are two roles on the platform:

- **Organiser** — creates, edits and deletes events; manages categories per event; views who has enrolled; captures results.
- **Participant** — browses events and categories, enrols into a category, views their own enrolments and results, and updates their own profile.

The database schema matches the Part 1 ERD/SQL script exactly.

##  Features

-  Session-based authentication with hashed passwords (`PasswordHasher<User>`)
-  Role enforcement on every protected endpoint, plus per-resource ownership checks
-  Full EF Core Code-First data model matching the Part 1 database design
-  Swagger UI with XML-comment-driven endpoint descriptions
-  xUnit test suite running against EF Core InMemory (no SQL Server needed for CI)
-  GitHub Actions pipeline: restore → build → test on every push

##  Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 Web API (controller-based) |
| ORM | Entity Framework Core 8, Code-First, SQL Server provider |
| Auth | `Microsoft.AspNetCore.Session` (server-side session, no JWT) |
| Password hashing | `Microsoft.AspNetCore.Identity.PasswordHasher<User>` |
| API docs | Swagger / Swashbuckle |
| Testing | xUnit + EF Core InMemory provider |
| CI/CD | GitHub Actions |

##  Project Structure
```
RaceDay-API/
├── RaceDay.API/
│   ├── Controllers/        AuthController, UsersController, EventsController,
│   │                        CategoriesController, EnrolmentsController, ResultsController
│   ├── Models/              User, Event, Category, Enrolment, Result, RouteWeatherInfo
│   ├── DTOs/                Request/response shapes, grouped by feature
│   ├── Data/                RaceDayContext (EF Core DbContext)
│   ├── Services/            IPasswordHashService / PasswordHashService
│   ├── Program.cs
│   └── appsettings.json
├── RaceDay.Tests/           xUnit test project (EF Core InMemory, no SQL Server needed)
├── .github/workflows/       dotnet-ci.yml — build + test on every push
├── Documentation/           CI green build screenshot
└── README.md
```

---

##  Database Schema

| Entity | Purpose |
|---|---|
| `Users` | Organisers and Participants, one table, discriminated by `Role` |
| `Events` | Owned by an Organiser; date, location, description |
| `Categories` | e.g. "10km Fun Run" under a specific Event; entry fee, capacity |
| `Enrolments` | Links a Participant to a Category |
| `Results` | Finish time / position, captured by the Organiser per Enrolment |
| `RouteWeatherInfo` | Race-day weather and route info per Event |

Key constraints enforced at the EF Core level: unique email per user, unique enrolment per participant/category pair, restrict-delete on foreign keys to avoid cascade cycles.

---

##  Getting Started

### Prerequisites

- Visual Studio 2022 (ASP.NET and web development workload) or the .NET 8 SDK
- SQL Server / SQL Server Express / LocalDB

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR-USERNAME/YOUR-REPO.git
   cd YOUR-REPO
   ```

   2. **Open `RaceDay.sln`** in Visual Studio, or work from the terminal.

3. **Check the connection string** in `RaceDay.API/appsettings.json` — defaults to LocalDB:
   ```
   Server=(localdb)\MSSQLLocalDB;Database=RaceDay;Trusted_Connection=True;...
   ```

4. **Create the database with EF Core migrations:**
   ```bash
   cd RaceDay.API
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the API:**
   ```bash
   dotnet run
   ```
   Your browser should open straight to Swagger at `/swagger`.

6. **Register and log in through Swagger** — `POST /api/auth/register`, then `POST /api/auth/login`. Swagger keeps the session cookie between calls, so once you're logged in you can try the role-restricted endpoints straight away.

---
##  Roles & Access Control

| Area | Organiser | Participant |
|---|---|---|
| Profile | View/update own | View/update own |
| Events | Create, update, delete own; view all | View all |
| Categories | Create/update/delete for own events; view all | View all |
| Enrolments | View enrolments for own events | Enrol, view own, cancel own |
| Results | Capture for own events | View own only |
| Weather/Route info | Add/update for own events | View |

Every write endpoint checks the session first (`401` if nobody's logged in), then the role (`403` if it's the wrong role), and — for Events/Categories/Enrolments/Results — that the logged-in Organiser actually owns the parent event before allowing the change.

##  API Reference

Full interactive documentation is available via Swagger UI at `/swagger` once the API is running. Summary below:

### Auth — `/api/auth`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/register` | — | Register a new Organiser or Participant |
| POST | `/login` | — | Log in and start a session |
| POST | `/logout` | — | Clear the current session |

### Users — `/api/users`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/me` | Logged in | Get own profile |
| PUT | `/me` | Logged in | Update own profile |

### Events — `/api/events`

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/` | — | List all events (filter by location/date) |
| GET | `/{id}` | — | Get a single event with categories |
| POST | `/` | Organiser | Create an event |
| PUT | `/{id}` | Organiser (owner) | Update an event |
| DELETE | `/{id}` | Organiser (owner) | Delete an event |
| GET | `/{id}/categories` | — | List categories for an event |
| POST | `/{id}/categories` | Organiser (owner) | Add a category to an event |
| GET | `/{id}/enrolments` | Organiser (owner) | List enrolments for an event |
| GET | `/{id}/results` | Organiser (owner) | List results for an event |
| GET | `/{id}/weather` | — | Get weather/route info |
| POST | `/{id}/weather` | Organiser (owner) | Add/update weather/route info |

### Categories — `/api/categories`

| Method | Route | Auth | Description |
|---|---|---|---|
| PUT | `/{id}` | Organiser (owner) | Update a category |
| DELETE | `/{id}` | Organiser (owner) | Delete a category |

### Enrolments — `/api/enrolments`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/` | Participant | Enrol into a category |
| GET | `/me` | Participant | List own enrolments |
| DELETE | `/{id}` | Participant (own) | Cancel own enrolment |

### Results — `/api/results`

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/` | Organiser (owner) | Capture a result for an enrolment |
| GET | `/me` | Participant | View own results |

---

##  Testing

Run the full suite:

```bash
dotnet test
```

Tests run against the EF Core InMemory provider, so CI doesn't need an actual SQL Server instance.

**Coverage includes:**
- **Authentication** — successful registration, duplicate email rejected, successful login starts a session, wrong password rejected
- **Events** — Organiser can create an event, Participant is rejected (403), anonymous request rejected (401), public events list works without logging in
- **Enrolments** — Participant can enrol and the record links correctly, Organiser is rejected, duplicate enrolment in the same category is rejected

---

##  CI/CD

`.github/workflows/dotnet-ci.yml` runs on every push and pull request to `main`: restores dependencies, builds in Release mode, then runs the full test suite.

**Green build:**

![CI green build](Documentation/ci-green-build.png)

---
