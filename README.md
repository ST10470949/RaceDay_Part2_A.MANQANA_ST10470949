# RaceDay — Part 2: RESTful API

**Module:** PROG6212 — Programming 2B
**Student:** A. Manqana (ST10470949)
**Part:** 2 of 3 — RESTful API Development (100 Marks)

This is Part 2 of the RaceDay Portfolio of Evidence. Part 1 was the planning stage (ERD,
API endpoint plan, SQL script). This part turns that plan into a working ASP.NET Core Web
API, backed by SQL Server through EF Core, with session-based authentication, role
enforcement, Swagger documentation, unit tests and a GitHub Actions CI/CD pipeline.

Part 3 will be an MVC front-end that consumes this API.

## System Description

RaceDay is an event management platform for South African road running, walking and
cycling events. There are two roles:

- **Organiser** — creates, edits and deletes events; manages categories per event; views
  who has enrolled; captures results.
- **Participant** — browses events and categories, enrols into a category, views their own
  enrolments and results, and updates their own profile.

The database schema matches the Part 1 ERD/SQL script exactly: `Users`, `Events`,
`Categories`, `Enrolments`, `Results`, `RouteWeatherInfo`.

## Project Structure

```
RaceDay_Part2_A.MANQANA_ST10470949/
├── RaceDay.API/
│   ├── Controllers/        AuthController, UsersController, EventsController,
│   │                        CategoriesController, EnrolmentsController, ResultsController
│   ├── Models/              User, Event, Category, Enrolment, Result, RouteWeatherInfo
│   ├── DTOs/                Request/response shapes, grouped by feature
│   ├── Data/                RaceDayContext (EF Core DbContext)
│   ├── Services/            IPasswordHashService / PasswordHashService - password hashing,
│   │                        injected into AuthController rather than instantiated inline
│   ├── Program.cs
│   └── appsettings.json
├── RaceDay.Tests/           xUnit test project (EF Core InMemory, no SQL Server needed)
├── .github/workflows/       dotnet-ci.yml — build + test on every push
├── Documentation/           CI green build screenshot goes here
└── README.md
```

## Technologies Used

- ASP.NET Core 8 Web API (controller-based, not minimal APIs)
- Entity Framework Core 8, Code-First, SQL Server provider
- Session-based authentication (`Microsoft.AspNetCore.Session`) — no JWT, the server
  keeps track of who's logged in via a session cookie that stores `UserId` and `Role`
- `PasswordHasher<User>` (from `Microsoft.AspNetCore.Identity`) for password hashing
- Swagger / Swashbuckle for API documentation and manual testing
- xUnit + EF Core InMemory provider for unit tests
- GitHub Actions for CI/CD

## How to Set Up and Run

1. **Install prerequisites**
   - Visual Studio 2022 with the "ASP.NET and web development" workload
   - SQL Server / SQL Server Express / LocalDB
   - .NET 8 SDK

2. **Clone the repository and open `RaceDay.sln`** in Visual Studio.

3. **Check the connection string** in `RaceDay.API/appsettings.json`. It defaults to
   LocalDB:
   ```
   Server=(localdb)\MSSQLLocalDB;Database=RaceDay;Trusted_Connection=True;...
   ```
   Change it if you're pointing at a different SQL Server instance.

4. **Create the database with EF Core migrations.** In the Package Manager Console
   (with `RaceDay.API` set as the default project):
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
   or from the terminal, inside `RaceDay.API/`:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   This creates the same tables as the Part 1 SQL script (EF Core generates them from
   the model classes in `Models/`, Code-First).

5. **Run the API** (F5, or `dotnet run` inside `RaceDay.API/`). Your browser should open
   straight to Swagger at `/swagger`.

6. **Register and log in through Swagger** — `POST /api/auth/register`, then
   `POST /api/auth/login`. Swagger keeps the session cookie between calls automatically,
   so once you're logged in you can try the role-restricted endpoints straight away.

## Roles and Access

| Area | Organiser | Participant |
|---|---|---|
| Profile | View/update own | View/update own |
| Events | Create, update, delete own; view all | View all |
| Categories | Create/update/delete for own events; view all | View all |
| Enrolments | View enrolments for own events | Enrol, view own, cancel own |
| Results | Capture for own events | View own only |
| Weather/Route info | Add/update for own events | View |

Every write endpoint checks the session first (`401` if nobody's logged in), then the
role (`403` if it's the wrong role), and — for Events/Categories/Enrolments/Results —
that the logged-in Organiser actually owns the parent event before allowing the change.

## Running the Tests

In Visual Studio: **Test → Test Explorer → Run All Tests**, or from the terminal:
```
dotnet test
```

Tests cover:
- **Authentication** — successful registration, duplicate email rejected, successful
  login starts a session, wrong password is rejected.
- **Events** — Organiser can create an event, Participant is rejected (403), an
  anonymous request is rejected (401), the public events list works without logging in.
- **Enrolments** — Participant can enrol and the record links the right Participant +
  Category, Organiser is rejected, enrolling twice in the same category is rejected.

They run against the EF Core InMemory provider, so CI doesn't need an actual SQL Server
instance to run them.

## CI/CD

`.github/workflows/dotnet-ci.yml` runs on every push: restores dependencies, builds in
Release mode, then runs the full test suite.

**Green build screenshot:** `Documentation/ci-green-build.png` — insert after your first
successful push.

## Video Walkthrough

**YouTube (unlisted):** _add your link here_

The video covers: project structure, the database, running the API, Swagger, register/
login, role restrictions, Events/Categories/Enrolments/Results, unit tests, and the
GitHub Actions green build.
