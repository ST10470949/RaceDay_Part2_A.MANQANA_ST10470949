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

