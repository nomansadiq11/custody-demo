# Custody Management API (.NET)

CRUD API for custody management using ASP.NET Core, EF Core, PostgreSQL, Docker Compose, request correlation IDs, logging, and global exception handling.

## Features

- Full CRUD for custody records
- PostgreSQL persistence with EF Core
- Global exception handling middleware
- Correlation ID middleware (`X-Correlation-ID`) on every request/response
- Structured application logging with scope support
- Dockerized API + PostgreSQL stack

## API Endpoints

- `GET /api/custodyrecords`
- `GET /api/custodyrecords/{id}`
- `POST /api/custodyrecords`
- `PUT /api/custodyrecords/{id}`
- `DELETE /api/custodyrecords/{id}`

Example JSON body:

```json
{
	"personName": "John Doe",
	"caseNumber": "CASE-2026-001",
	"arrestedAtUtc": "2026-05-21T08:30:00Z",
	"facility": "Central Detention Facility",
	"status": "InCustody"
}
```

## Run Locally

```bash
cd CustodyManagementApi
dotnet restore
dotnet run
```

API default URL (from launch profile): `http://localhost:5000` or `https://localhost:5001`.

## Run With Docker Compose

From repository root:

```bash
docker compose up --build
```

API URL:

- `http://localhost:8080`

Stop services:

```bash
docker compose down
```

Reset database volume:

```bash
docker compose down -v
```

## Correlation ID

- Send optional header: `X-Correlation-ID: <your-id>`
- If not provided, API generates one automatically
- Same value is returned in response header `X-Correlation-ID`
- Correlation ID is included in logging scope and error payloads