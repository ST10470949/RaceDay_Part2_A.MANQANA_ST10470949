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
