# 🏆 Game Leaderboard API

A robust, production-ready REST API for game leaderboards built with **.NET 9/10**, **Clean Architecture**, and **Docker**.

Perfectly suited for mobile/hyper-casual games where anonymous authentication (via Device ID) is required.

## 🚀 Features

- **Anonymous Auth:** Login or register seamlessly using a unique Device ID (no emails/passwords required).
- **Leaderboard System:** Submit scores, get global Top-N players, and calculate current player rank.
- **Clean Architecture:** Domain, Application, Infrastructure, and API layers strictly separated.
- **Modern .NET Stack:** Minimal-style Program.cs, `IExceptionHandler` for global errors, Native OpenAPI.
- **Scalar UI:** Beautiful, interactive API documentation (replaced Swagger).
- **Containerized:** Run PostgreSQL and the API with a single Docker command.

## 🛠 Tech Stack

- **Framework:** .NET 9/10, ASP.NET Core Web API
- **Database:** PostgreSQL + Entity Framework Core (Code First), Redis
- **Validation:** FluentValidation
- **Authentication:** JWT Bearer
- **Documentation:** Scalar UI / OpenAPI
- **Testing:** xUnit, Moq, FluentAssertions, EF Core In-Memory

## 🎮 How to Run

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.
- .NET 9.0 SDK (or newer).

### 1. Start the Database
Open terminal in the root folder and run:
```bash
docker-compose up -d
