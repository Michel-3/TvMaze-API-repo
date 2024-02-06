# TvMazeAPI

TvMazeAPI is a C# application that interacts with the TVMaze API to retrieve information about TV shows and actors. The application calculates actor percentages based on show data and provides relevant insights.

## Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Folder Structure](#folder-structure)
- [Dependencies](#dependencies)
- [Contributing](#contributing)
- [License](#license)

## Project Overview

This project leverages the TVMaze API to gather information about TV shows, their cast, and calculates actor percentages based on show data. The application is built using C# and ASP.NET Core.

## Architecture

The project follows a layered architecture to ensure modularity, separation of concerns, and maintainability. Key layers include:

- **Controller (Presentation Layer):** Handles user input, manages interactions with business logic, and presents results.

- **Core (Business Layer):**
  - `Models`: Defines data structures.
  - `Services`: Contains interfaces and implementations for business services.
    - `Interfaces`: Declares interfaces for services.
    - `Implementations`: Implements business logic for services.

- **Repository (Data Access Layer):** Interacts with the database and manages data access.

## Getting Started

Follow these instructions to set up the project locally.

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)

### Installation

1. Clone the repository.
   ```bash
   git clone https://github.com/your-username/TvMazeAPI.git
2. Navigate to the project folder.
   cd TvMazeAPI
3. Build the project
   dotnet build
4. Run the application
   dotnet run

## Usage

The application provides an API endpoint to retrieve shows and calculate actor percentages.

- **Endpoint:** `GET /TvMaze/shows`
- **Parameters:** `month` and `year`
- **Example:** [http://localhost:5000/TvMaze/shows?month=1&year=2022](http://localhost:5000/TvMaze/shows?month=1&year=2022)

## Folder Structure

The project follows a structured folder layout:

- `Controllers`: Contains API controllers.
- `Core`: Houses the business logic.
  - `Models`: Defines data structures.
  - `Services`: Contains interfaces and implementations for business services.
    - `Interfaces`: Declares interfaces for services.
    - `Implementations`: Implements business logic for services.
- `Repository`: Manages data access.
- `Migrations`: Contains database migration files.
- `Program.cs`: Entry point of the application.

## Dependencies

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient)
- [Swagger/OpenAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
