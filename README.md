# MDW-Clase2

Proyecto académico de desarrollo web: una API ASP.NET Core para administrar
cursos y profesores, organizada en proyectos de dominio, aplicación,
infraestructura y presentación.

## Requisitos

- SDK de .NET 10
- Docker Desktop o Docker Engine

## Iniciar SQL Server

Crea e inicia un contenedor de SQL Server 2022 con un volumen persistente para
los datos:

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

Reemplaza `YourStrong!Passw0rd` por una contraseña segura propia (SQL Server
requiere una contraseña segura para el usuario `sa`). La opción
`--platform linux/amd64` también permite ejecutar la imagen de SQL Server en
equipos Apple Silicon mediante la emulación de Docker.

Espera a que SQL Server esté listo:

```bash
until docker exec mdw-sqlserver \
  /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'YourStrong!Passw0rd' -C -Q 'SELECT 1' \
  >/dev/null 2>&1; do
  sleep 2
done
```

La aplicación no crea automáticamente el esquema de la base de datos. Crea la
base y las tablas que espera Entity Framework Core:

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

## Compilar e iniciar la API

Desde la raíz del repositorio, configura la cadena de conexión con User Secrets
de .NET. Usa la misma contraseña que configuraste al iniciar SQL Server:

```bash
dotnet user-secrets set "ConnectionStrings:Default" \
  "Server=localhost,1433;Database=MDWClase2;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=True;TrustServerCertificate=True" \
  --project Clase3.Presentation
dotnet restore MDW-Clase2.sln
dotnet build MDW-Clase2.sln
dotnet run --project Clase3.Presentation
```

La API estará disponible en la URL que muestre `dotnet run` (por defecto,
`http://localhost:5126`). En el entorno Development, puedes consultar la
especificación OpenAPI en `http://localhost:5126/openapi/v1.json` (o en
`https://localhost:7038/openapi/v1.json` si usas el perfil HTTPS).

El proyecto no tiene configurada la interfaz gráfica de Swagger UI, por lo que
la ruta `/swagger` no está disponible. La documentación se ofrece como un
archivo JSON OpenAPI.

Para detener y volver a iniciar el contenedor de la base de datos:

```bash
docker stop mdw-sqlserver
docker start mdw-sqlserver
```

Los datos de la base se conservan en el volumen de Docker
`mdw-sqlserver-data`.

## Proyectos

- `Clase2.Domain`: entidades de cursos y profesores.
- `Clase2.Application`: comandos, consultas y abstracciones de repositorios.
- `Clase5.Infrastructure`: contexto de Entity Framework Core para SQL Server
  y repositorios.
- `Clase3.Presentation`: controladores de la API ASP.NET Core.

## Rutas de la API

- `api/cursos`: crear cursos; consultar, actualizar o eliminar un curso por ID.
- `api/profesores`: crear profesores; consultar, actualizar o eliminar un
  profesor por ID.
