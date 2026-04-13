# 🌍 INF5120 Arch0604 API

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)]()
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Tests](https://img.shields.io/badge/tests-passing-brightgreen)]()
[![Coverage](https://img.shields.io/badge/coverage-80%25+-yellowgreen)]()
[![License](https://img.shields.io/badge/license-MIT-blue)]()
[![CI](https://github.com/edjnolasco/inf-arch0604/actions/workflows/ci.yml/badge.svg)](https://github.com/edjnolasco/inf-arch0604/actions/workflows/ci.yml)

API REST desarrollada en ASP.NET Core para la gestión de países, implementando una arquitectura en capas con buenas prácticas de ingeniería de software: validación, manejo de errores, logging, pruebas unitarias y documentación con Swagger.

------------------------------------------------------------------------

## 🧩 Arquitectura

El proyecto sigue una arquitectura basada en capas:

Controller → Service → Repository → DbContext

## 📁 Estructura

```text
INF._5120.Arch0604
├── Api
│ ├── Controllers
│ ├── Middleware
│ ├── Common
│ └── Extensions
│
├── Application
│ ├── Services
│ ├── Interfaces
│ ├── DTOs
│ └── Common
│
├── Infrastructure
│ ├── Persistence
│ └── Repositories
│
├── Domain
│ └── Entities
│
└── Tests
```

### Capas:

-   API
    -   Controladores (endpoints REST)
    -   Middleware global de excepciones
    -   Configuración Swagger
-   Application
    -   Servicios de negocio (`CountryService`)
    -   Interfaces (`ICountryService`, `ICountryRepository`)
    -   DTOs (Request / Response)
    -   `ServiceResult<T>` para manejo de errores tipados
-   Infrastructure
    -   Implementación de repositorios
    -   DbContext (Entity Framework Core)
-   Domain
    -   Entidades del dominio (`Country`)

------------------------------------------------------------------------

## ⚙️ Tecnologías utilizadas

-   .NET 8 / ASP.NET Core
-   Entity Framework Core
-   SQL Server
-   Swagger / OpenAPI
-   xUnit (testing)
-   InMemory Database (tests)
-   Logging con `ILogger`
-   Middleware global de excepciones

------------------------------------------------------------------------

## 📦 Funcionalidades

CRUD completo de países:

-   GET /api/Country
-   GET /api/Country/{id}
-   POST /api/Country
-   PUT /api/Country/{id}
-   DELETE /api/Country/{id}

------------------------------------------------------------------------

## 🧠 Manejo de errores

-   ServiceResult`<T>`{=html}
-   Middleware global

Ejemplo:

{ "statusCode": 500, "message": "Ocurrió un error interno en el
servidor.", "traceId": "...", "timestampUtc": "..." }

------------------------------------------------------------------------

## 📝 Logging

Se utiliza `ILogger<T>` para registrar:

-   Operaciones CRUD
-   Validaciones fallidas
-   Duplicados
-   Errores

------------------------------------------------------------------------

## 🧪 Pruebas

Incluye pruebas unitarias para:

-   Controller
-   Service

------------------------------------------------------------------------

## 🔍 Swagger

Disponible en:

https://localhost:{puerto}/swagger

------------------------------------------------------------------------

## 🛠️ Configuración

Editar en appsettings.json:

"ConnectionStrings": { "DefaultConnection":
"Server=.;Database=INF5120Db;Trusted_Connection=True;TrustServerCertificate=True;"
}

------------------------------------------------------------------------

## 🚀 CI/CD

Pipeline con GitHub Actions:

-   build
-   tests
-   coverage (XPlat Code Coverage)

------------------------------------------------------------------------

## ▶️ Ejecución

dotnet build dotnet run

------------------------------------------------------------------------

## 📌 Estado

✔ CRUD\
✔ Swagger\
✔ Logging\
✔ Middleware\
✔ Tests\
✔ CI/CD

------------------------------------------------------------------------

## 📚 Autor

Edwin José Nolasco. Proyecto de la asignatura INF-5120.
