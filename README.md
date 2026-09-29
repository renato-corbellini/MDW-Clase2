# MDW-Clase2

Academic project for web development: an ASP.NET Core API for managing courses
and professors, organized into domain, application, infrastructure, and
presentation projects.

## Requirements

- .NET 10 SDK
- SQL Server

## Build and run

Set the database connection string locally with .NET User Secrets (do not
commit credentials):

```bash
dotnet user-secrets set "ConnectionStrings:Default" \
  "Server=localhost;Database=MDWClase2;Trusted_Connection=True;TrustServerCertificate=True" \
  --project Clase3.Presentation
dotnet run --project Clase3.Presentation
```

The API expects the `Cursos` and `Profesores` tables to exist in the configured
database. In Development, the OpenAPI document is available at
`/openapi/v1.json`.

## Projects

- `Clase2.Domain` — course and professor entities.
- `Clase2.Application` — commands, queries, and repository abstractions.
- `Clase5.Infrastructure` — Entity Framework Core SQL Server context and
  repositories.
- `Clase3.Presentation` — ASP.NET Core API controllers.

## API routes

- `api/cursos` — create courses; retrieve, update, or delete a course by ID.
- `api/profesores` — create professors; retrieve, update, or delete a professor
  by ID.
