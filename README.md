# MDW-Clase2

Academic project for web development: an ASP.NET Core API for managing courses
and professors, organized into domain, application, infrastructure, and
presentation projects.

## Requirements

- .NET 10 SDK
- Docker Desktop or Docker Engine

## Start SQL Server

Create and start a SQL Server 2022 container with a persistent data volume:

```bash
docker run \
  --name mdw-sqlserver \
  --hostname mdw-sqlserver \
  --platform linux/amd64 \
  --publish 1433:1433 \
  --env ACCEPT_EULA=Y \
  --env MSSQL_SA_PASSWORD='YourStrong!Passw0rd' \
  --volume mdw-sqlserver-data:/var/opt/mssql \
  --detach \
  mcr.microsoft.com/mssql/server:2022-latest
```

Replace `YourStrong!Passw0rd` with your own strong password (SQL Server
requires a strong `sa` password). The `--platform linux/amd64` option also
allows the SQL Server image to run on Apple Silicon using Docker's emulation.
Wait for SQL Server to become ready:

```bash
until docker exec mdw-sqlserver \
  /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'YourStrong!Passw0rd' -C -Q 'SELECT 1' \
  >/dev/null 2>&1; do
  sleep 2
done
```

The application does not create the database schema automatically. Create the
database and tables expected by Entity Framework Core:

```bash
docker exec -i mdw-sqlserver \
  /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'YourStrong!Passw0rd' -C <<'SQL'
CREATE DATABASE MDWClase2;
GO
USE MDWClase2;
GO
CREATE TABLE Cursos (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    Name nvarchar(200) NOT NULL,
    Credits int NOT NULL
);
CREATE TABLE Profesores (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    Nombre nvarchar(100) NOT NULL,
    Apellido nvarchar(100) NOT NULL,
    Legajo nvarchar(20) NOT NULL
);
CREATE UNIQUE INDEX IX_Profesores_Legajo ON Profesores (Legajo);
GO
SQL
```

## Build and run the API

From the repository root, configure the connection string using .NET User
Secrets. Use the same password set when starting SQL Server:

```bash
dotnet user-secrets set "ConnectionStrings:Default" \
  "Server=localhost,1433;Database=MDWClase2;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=True;TrustServerCertificate=True" \
  --project Clase3.Presentation
dotnet restore MDW-Clase2.sln
dotnet build MDW-Clase2.sln
dotnet run --project Clase3.Presentation
```

The API listens at the URL printed by `dotnet run` (by default,
`http://localhost:5126`). In Development, the OpenAPI document is available at
`http://localhost:5126/openapi/v1.json`.

To stop and restart the database container:

```bash
docker stop mdw-sqlserver
docker start mdw-sqlserver
```

The database data remains in the `mdw-sqlserver-data` Docker volume.

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
