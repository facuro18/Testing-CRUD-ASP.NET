# Quick Setup Guide

## Prerequisites

- Install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Setup Commands (Run in order)

```bash
# 1. Clone the repository
git clone <your-repo-url>
cd Testing-CRUD

# 2. Restore NuGet packages (creates obj/ folder)
dotnet restore

# 3. Build the project (creates bin/ folder)
dotnet build

# 4. Run the application
dotnet run
```

## Alternative: One-Step Run

```bash
# This does restore + build + run automatically
dotnet run
```

## Access the API

- Application: http://localhost:5174
- API endpoint: http://localhost:5174/WeatherForecast
- OpenAPI Scalar: http://localhost:5174/api/docs/
