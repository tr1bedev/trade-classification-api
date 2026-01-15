# Trade Classification API

## Overview
ASP.NET Core 8 API for classifying trade risks. Layered architecture for extensibility.

## Endpoints
- POST /api/trades/classify: Classifies trades. Body: List of {value, clientSector}.
- POST /api/trades/analyze: Analyzes with summary. Body: List of {value, clientSector, clientId}.

## Running
- `dotnet restore`
- `cd Api && dotnet run`
- Access Swagger at https://localhost:7241/swagger

## Decisions
- Interfaces for classification rules (extensible).
- LINQ/Dictionaries for efficient processing.
- Basic logging with ILogger.
- Tests cover 80%+ code.

## Future Improvements
- Add more rules via strategy pattern.
- Database integration.
