# Repository Guidelines

## Project Structure & Module Organization
`BackEnd/` contains the ASP.NET Core 8 API: `Controllers/` expose REST endpoints, `Models/` host EF entities plus DTOs, `Repositories/` and `Services/` encode business logic, while `Data/`, `Migrations/`, and `Seeds/` coordinate persistence and sample data. Shared helpers sit in `Utilities/`, and API/GraphQL notes live in `Document/`. Observability assets stay in `Opentelemetry/` (Grafana, Prometheus, Loki, Tempo, Alertmanager, nginx, Blackbox, Postgres init SQL). `docker-compose.yml` is the single entry point for the API + PostgreSQL stack—extend it whenever a new containerized dependency is added.

## Build, Test, and Development Commands
- `docker compose up -d --build` — builds `BackEnd/` and boots Postgres with volumes.
- `docker compose logs -f api` and `docker compose ps` — inspect container health.
- `cd BackEnd && dotnet build` — compile and lint via Roslyn analyzers.
- `cd BackEnd && dotnet run` — API on http://localhost:8080 for breakpoint-driven debugging.

## Coding Style & Naming Conventions
Use 4-space indentation, `PascalCase` for public types, `camelCase` for locals, and suffix async methods with `Async`. DTOs stay under `Models/Dtos` (e.g., `ProductDto.Request`). Prefer dependency-injected services over static helpers, keep configuration in `appsettings*.json`, and run your editor’s C# formatter or `dotnet format` before committing.

## Testing Guidelines
Add unit/integration projects under `BackEnd/Tests` (create when needed) using xUnit or NUnit. Mirror the `Controllers/Services` layout inside tests, name classes `{TypeUnderTest}Tests`, and follow `MethodUnderTest_ExpectedBehavior_Scenario` for methods. Execute `cd BackEnd && dotnet test` locally; long-running suites can be tagged with `[Trait("Category", "Integration")]` and filtered via `dotnet test --filter Category=Integration`.

## Commit & Pull Request Guidelines
Git history follows Conventional Commits (`feat: ...`, `fix: ...`). Keep subjects imperative and under ~60 chars, and describe breaking changes or migrations inside the commit body. PRs should summarize functional impact, affected endpoints, schema/migration updates (`BackEnd/Migrations`), telemetry changes (`Opentelemetry/*`), and manual verification steps or screenshots/curl samples when contracts change. Link the relevant issue ticket and call out pending follow-up work.

## Security & Configuration Tips
Default secrets (`POSTGRES_PASSWORD=adminpassword`, Grafana `admin/admin`) are local-only; override via `.env` or Docker secrets before staging. Store JWT keys and additional connection strings outside source control (e.g., `appsettings.Development.json`). When exposing new ports or telemetry streams, update both `README.md` and the corresponding `Opentelemetry/` configs so operations dashboards stay accurate.
