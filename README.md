# Testing-CRUD

A simple ASP.NET Core Web API project for testing CRUD operations.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone <your-repository-url>
   cd Testing-CRUD
   ```

2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

3. **Run the application:**

   ```bash
   dotnet run
   ```

4. **Access the API:**
   - Application: `http://localhost:5174`
   - OpenAPI specification: `http://localhost:5174/openapi/v1.json`
   - Test API: `http://localhost:5174/WeatherForecast`

## Project Structure

```
📁 Testing-CRUD/
├── 📄 Program.cs                    # Application entry point
├── 📄 Testing-CRUD.csproj          # Project configuration
├── 📄 WeatherForecast.cs           # Data model
├── 📁 Controllers/
│   └── 📄 WeatherForecastController.cs  # API controller
└── 📁 Properties/
    └── 📄 launchSettings.json      # Launch settings
```

## API Endpoints

- `GET /WeatherForecast` - Returns sample weather forecast data

## Development

- The application runs on `http://localhost:5174` by default
- OpenAPI documentation is available in development mode
- Hot reload is enabled for development
