# Weather Application
Project URL: https://roadmap.sh/projects/weather-api-wrapper-service

Weather API is a simple ASP.NET Web API that acts as a wrapper around a third-party weather provider.

## Features 
- Fetch current weather data by city
- Abstracts third-party weather API complexity
- Error handling for failed or invalid requests
- JSON-based responses suitable for frontend or other services

## Prerequisites 
- .NET 8 SDK 
- An API key from OpenWeatherMap (https://www.visualcrossing.com/weather-api/)

## Configuration
1. Apply the API key in your configuration file (e.g., `appsettings.json`) or as an environment variable.

## Getting Started 
1. Clone the repository or download the source code. 
2. Open a terminal and navigate to the project directory. 
3. Restore dependencies: ```dotnet restore```
4. Run the application using the command: ```dotnet run```