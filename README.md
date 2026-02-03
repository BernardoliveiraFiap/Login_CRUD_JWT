# iPool Backend

API REST em .NET com SQL Server, JWT e camadas Controller/Service/Repository.

## Requisitos

- .NET SDK 10
- SQL Server (LocalDB ou SQL Express)

## Configuração

Edite a connection string e o JWT em:

- appsettings.json
- appsettings.Development.json

## Executar

```bash
cd ipoolBackend
dotnet run
```

## Scripts SQL

Veja os scripts em `ipoolBackend/scripts`.

## Documentação (Scalar)

http://localhost:5000/scalar/v1

## Endpoints

- POST /api/auth/register
- POST /api/auth/login
- GET /api/users/me (JWT)
- GET /api/health
- GET /api/health/db
- GET /api/info