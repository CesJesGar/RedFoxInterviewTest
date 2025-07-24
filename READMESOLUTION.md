->RedFox Technical Test Solution

-Solución de la Prueba Técnica de RedFox

-Summary

In this challenge, I built a .NET 8 web API following Clean Architecture principles and applying CQRS with MediatR. The solution seeds the database from JSONPlaceholder, extending each user with full `Address` and `Geo` data, and stores everything in SQLite via Entity Framework Core. I also implemented full CRUD endpoints using Minimal APIs and enforced input validation with FluentValidation.

-Resumen

En este reto desarrollé una API web en .NET 8 siguiendo principios de Clean Architecture y usando CQRS con MediatR. La solución inicia la base de datos desde JSONPlaceholder, extendiendo cada usuario con datos completos de `Address` y `Geo`, y guarda todo en SQLite con EF Core. Además, implementé endpoints CRUD con Minimal APIs y validación de entrada con FluentValidation.

Architecture Decisions

Entities vs Value Objects: Chose entities for `Address` and `Geo` to leverage EF Core 1:1 relationships and lifecycle management (cascading deletes, change tracking). Kept `Company` and `User` as entities.
CQRS & MediatR: Separated read queries and write commands into distinct handlers for clear responsibilities.
Minimal API: Used Minimal APIs in `Program.cs` to reduce boilerplate code.
AutoMapper: Centralized mapping logic in `MappingProfile` to convert between domain entities and DTOs.
FluentValidation: Integrated for declarative, testable input validation of DTOs.

Decisiones de Arquitectura

Entidades vs Objetos de Valor: Usé entidades para `Address` y `Geo` para aprovechar relaciones 1:1 de EF Core y gestión de ciclo de vida. Mantuve `Company` y `User` como entidades.
CQRS & MediatR: Separé consultas de lectura y comandos de escritura en handlers distintos para responsabilidades claras.
Minimal API: Utilicé Minimal APIs en `Program.cs` para minimizar código repetitivo.
AutoMapper: Centralicé la lógica de mapeo en `MappingProfile` para transformar entre entidades de dominio y DTOs.
FluentValidation: Integré para validación declarativa y testeable de DTOs.

CRUD Implementation & Validation

GET /users: Returns list of users with nested `address` and `geo`.
GET /users/{id}: Returns a single user or 404 if not found.
POST /users: Accepts `UserCreationDto`, validates required fields, email format, nested DTOs, returns 201 Created.
PUT /users/{id}: Accepts `UserUpdateDto`, validates input, updates in-place via EF Core, returns 200 OK or 404.
DELETE /users/{id}: Deletes user and cascades delete to `Address` and `Geo`, returns 204 No Content.

Validation covers non-null, non-empty fields, email format, and nested objects. Duplicate checks can be added with `MustAsync` if needed.

Implementación CRUD y Validación

GET /users: Devuelve lista de usuarios con `address` y `geo` anidados.
GET /users/{id}: Devuelve un usuario o 404 si no existe.
POST /users: Recibe `UserCreationDto`, valida campos obligatorios, formato de email y DTOs anidados, devuelve 201 Created.
PUT /users/{id}: Recibe `UserUpdateDto`, valida datos, actualiza in situ con EF Core, devuelve 200 OK o 404.
DELETE /users/{id}: Elimina el usuario y su `Address`/`Geo`, devuelve 204 No Content.

La validación cubre campos no nulos/vacíos, formato de email y objetos anidados. Se pueden añadir checks de duplicados con `MustAsync` si se desea.

Trade-offs & Assumptions

Entity vs Value Object: Although `Address` and `Geo` could be modeled as value objects, using entities simplified EF Core configuration and lifecycle.
Database duplicate checks: Skipped async uniqueness validation under time constraints.
Minimal API choice: Opted for speed and simplicity; controllers may scale better in larger codebases.

Compromisos y Suposiciones

Entidad vs Objeto de Valor: Aunque `Address` y `Geo` podrían ser objetos de valor, usar entidades simplificó la configuración en EF Core.
Chequeos de duplicados: Omisión de validación de unicidad en la base por tiempo.
Elección de Minimal API: Priorizó rapidez; en proyectos grandes, los controladores podrían ser más adecuados.

Testing Instructions

Build & Run: `dotnet build` and then `dotnet run --project RedFox.Api`.
Swagger UI: Navigate to `https://localhost:5105/swagger` to explore endpoints.
Validation Tests: Submit empty or invalid JSON to `POST /users` and `PUT /users/{id}` to see 400 responses.
CRUD Tests: Perform create, read, update, and delete operations to ensure database persistence in `ExtendedYankee2.db`.
DB Inspection: Open `ExtendedYankee2.db` with a SQLite viewer to verify tables and seeded data.

Instrucciones de Prueba

Compilar y Ejecutar: `dotnet build` y `dotnet run --project RedFox.Api`.
Swagger UI: Abre `https://localhost:5105/swagger` para explorar los endpoints.
Pruebas de validación: Envía JSON vacío o inválido a `POST /users` y `PUT /users/{id}` y verifica respuestas 400.
Pruebas CRUD: Realiza operaciones de crear, leer, actualizar y eliminar para comprobar la persistencia en `ExtendedYankee2.db`.
Inspección de BD: Abre `ExtendedYankee2.db` con un viewer SQLite para verificar tablas y datos de seed.

Time Breakdown

Domain modeling (`Address`/`Geo`): 1.5h
EF Core config & migrations: 2h
Data seeding & `DbInitWorker`: 1.5h
DTOs & AutoMapper mapping: 1h
CRUD endpoints implementation: 3h
Input validation (FluentValidation): 1.5h
Testing & Swagger verification: 1h
Code comments & documentation: 1h

Total: 12.5 hours

Desglose de Tiempo

Modelado de dominio (`Address`/`Geo`): 1.5h
Configuración EF Core y migraciones: 2h
Siembra de datos & `DbInitWorker`: 1.5h
DTOs y mapeo con AutoMapper: 1h
Implementación CRUD: 3h
Validación de entrada: 1.5h
Pruebas & verificación en Swagger: 1h
Comentarios de código & documentación: 1h

Total: 12.5 horas

Challenges Faced

Configuring EF Core 1:1 relationships for nested entities and seeding JSON data.
Resolving AutoMapper mapping errors for command vs DTO wrappers.
Integrating FluentValidation in Minimal API and resolving DI scanning issues.
Ensuring update logic modified existing entities without orphaning related data.

Desafíos Encontrados

- Configurar relaciones 1:1 en EF Core para entidades anidadas y siembra desde JSON.

- Resolver errores de AutoMapper entre comandos y DTOs.

- Integrar FluentValidation en Minimal API y solucionar inyección de dependencias.

- Garantizar que la lógica de actualización modificara entidades existentes sin crear datos huérfanos.

- Configuring EF Core 1:1 relationships for nested entities and seeding JSON data.

-Resolving AutoMapper mapping errors for commands vs DTO wrappers.

- Integrating FluentValidation in Minimal API and resolving DI scanning issues.

- Ensuring update logic modified existing entities without orphaning related data.
