# Phase 1: Q1 Project Scaffold & EF Core Models/DbContext

## Requirements

Create a new ASP.NET Core 8 Web API project at `solution/Q1_WebAPI`, define seven entity classes that match the exact schema from `given_pe_trial/1/database.sql` (Directors, Movies, Producers, Stars, Genres, Movie_Genre with composite key, Movie_Star with composite key), write a manual AppDbContext with Fluent API mappings (not scaffolded, not using code-first migrations), configure the connection string from `appsettings.json` under `ConnectionStrings:MyCnn`, and verify the DbContext can connect to a real SQL Server instance and query basic data without errors.

## Design Constraints

Preflight: No existing ASP.NET Core project in this repo to inherit conventions from (fresh scaffold); closest analog is the read-only reference `given_pe_trial/2/givenAPI` project, which uses: minimal hosting `Program.cs` (`builder.Services.AddControllers()`, `AddEndpointsApiExplorer()`, `AddSwaggerGen()`, CORS `AllowAnyOrigin/Header/Method`, `UseSwagger`/`UseSwaggerUI` in Development, `UseHttpsRedirection`, `MapControllers`), POCO models with `= null!` for non-nullable reference properties, and `[Route("api/[controller]")] [ApiController]` controllers extending `ControllerBase`. Q1_WebAPI follows the same shape: namespace `Q1_WebAPI`, `Q1_WebAPI.Models` (entities), `Q1_WebAPI.Models.Dtos` (DTOs), `Q1_WebAPI.Data` (DbContext), `Q1_WebAPI.Controllers`. Installed SDKs on this machine: .NET 8.0.311 and 9.0.314 (9.x is the default) — new projects must be scaffolded with `-f net8.0` explicitly or the `.csproj` `TargetFramework` must be corrected to `net8.0` afterward, per the exam's ".NET 8.0" requirement.

- Must not use EF Core scaffolding tools (`Scaffold-DbContext`) or code-first migrations (`Add-Migration`, `Update-Database`). The database schema is fixed and pre-created by the exam's SQL script; the DbContext is a read-write mapping layer only.
- Must only add NuGet packages `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Design`; no Automapper, Serilog, or identity frameworks.
- Composite key Fluent API configurations must use `.HasKey(e => new { e.PropertyA, e.PropertyB })` pattern explicitly; no data annotations or implicit conventions.
- Connection string must be read from `appsettings.json` under key `ConnectionStrings:MyCnn` at runtime; no hardcoded connection strings **in code**.
- **Allowed exception (SEC_NO_HARDCODED_SECRET):** the exam explicitly mandates the real connection string live in the base `appsettings.json` (not `appsettings.Development.json`), because the submission is published via `dotnet publish -c Release` and run by an external grading harness that is not guaranteed to set `ASPNETCORE_ENVIRONMENT=Development` — only the base `appsettings.json` is guaranteed to load in that scenario. The `sa`/`12345` credential is a local, single-machine exam database with no production exposure, not a real secret boundary. Storing it only in `appsettings.Development.json` (the generic best practice) would silently break the published Release build. This exception applies to this file/key only, not to future credentials in this project.
- Phase must not implement any HTTP endpoints; only DbContext setup.

## Steps

1. Create a shared solution file `solution/PE_Trial.sln` (`dotnet new sln -n PE_Trial -o solution`), then create the ASP.NET Core 8 Web API project at `solution/Q1_WebAPI` (`dotnet new webapi -o solution/Q1_WebAPI`) and add it to the solution (`dotnet sln solution/PE_Trial.sln add solution/Q1_WebAPI/Q1_WebAPI.csproj`). Configure `appsettings.json`, `appsettings.Development.json`, and `launchSettings.json` (port 5000). Phase 3 will add `Q2_MovieApp` to the same `.sln` so both projects open together in Visual Studio.

2. Install `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Design` NuGet packages into the project.

3. Write entity classes in `solution/Q1_WebAPI/Models/` folder: `Director.cs`, `Movie.cs`, `Producer.cs`, `Star.cs`, `Genre.cs`, `MovieGenre.cs`, `MovieStar.cs` with properties matching exact column names and data types from `database.sql` (e.g., Director has Id, FullName, Male (bool), Dob (DateTime), Nationality, Description). `Director` gets a `Movies` navigation collection; `Movie` gets nullable `Director` and `Producer` navigation references (see Step 4).

4. Create `solution/Q1_WebAPI/Data/AppDbContext.cs` inheriting from `DbContext`, define `DbSet<T>` properties for each entity, and implement `OnModelCreating()` method with Fluent API to configure composite keys, foreign key relationships, and any table name mappings (e.g., movie_genre → Movie_Genre).

   **Navigation properties are required** (Phase 2's `GetDirector` endpoint uses `.Include(d => d.Movies)`, and the director-with-movies response needs `producerName`/`directorName` on each movie — see spec.md example response for 1.2): add `Director.Movies` (collection) and `Movie.Director` / `Movie.Producer` (reference, both nullable since `Movies.ProducerId`/`Movies.DirectorId` allow null) navigation properties, and configure them explicitly in `OnModelCreating`:
   ```csharp
   modelBuilder.Entity<Movie>()
       .HasOne(m => m.Director)
       .WithMany(d => d.Movies)
       .HasForeignKey(m => m.DirectorId)
       .IsRequired(false);

   modelBuilder.Entity<Movie>()
       .HasOne(m => m.Producer)
       .WithMany()
       .HasForeignKey(m => m.ProducerId)
       .IsRequired(false);
   ```

5. Add AppDbContext to the dependency injection container in `Program.cs` using `services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MyCnn")))`.

6. Create or update `appsettings.json` to include connection string under `ConnectionStrings:MyCnn`. **The database name must be exactly `PE_PRN_Fall22B1`** — this is the name created by `given_pe_trial/1/database.sql` (`CREATE DATABASE [PE_PRN_Fall22B1]`), not a placeholder name. The user logs into their local SQL Server with **SQL Server Authentication** (`sa` / `12345`), not Windows auth, so the connection string must use `User Id=sa;Password=12345` and `TrustServerCertificate=true`:
   ```
   "Server=localhost;Database=PE_PRN_Fall22B1;User Id=sa;Password=12345;TrustServerCertificate=true;"
   ```
   The exact `Server=` value (`localhost`, `.`, `.\SQLEXPRESS`, etc.) depends on the user's actual SQL Server instance name — if `localhost` fails to connect, try `.` or check the instance name in SQL Server Configuration Manager / SSMS, and also confirm SQL Server Authentication mode ("Mixed Mode") is enabled on the instance (otherwise `sa` login will be rejected even with the correct password).

7. Run a simple validation: create a temporary endpoint or console test that queries `context.Directors.FirstOrDefault()` to confirm the DbContext connects to the real SQL Server instance and returns data; then remove the test code before committing.

## Success Criteria

- Q1_WebAPI project builds without errors and contains `Models/` folder with seven entity classes.
- `appsettings.json` includes a valid `ConnectionStrings:MyCnn` entry pointing to a local SQL Server instance.
- AppDbContext can be instantiated from `Program.cs` without exceptions; dependency injection for `IServiceCollection.AddDbContext()` is configured.
- A test query (e.g., `context.Directors.Count()`) executes against the real database and returns a non-negative integer, confirming schema mappings are correct.
- No migration files or scaffolded code artifacts exist in the project; all mappings are explicit in Fluent API.

## Quality and Testing State

- Quality gate: approved (`plans/pe-trial-2/quality/phase-01-q1-scaffold-and-efcore-quality-report.json`, receipt issued)
- Testing: passed (15/15 unit tests, `plans/pe-trial-2/tests/phase-01-q1-scaffold-and-efcore-test-report.json`)

## Risks

- **DbContext Mismatch to Schema:** If entity properties don't exactly match `database.sql` columns (e.g., property name case, missing columns, wrong data types), queries fail or return incomplete data. *Mitigation:* Cross-check entity definitions line-by-line against the SQL script before proceeding; run a test query immediately.
- **Composite Key Configuration Error:** Typo in `.HasKey(e => new { e.MovieId, e.GenreId })` (e.g., wrong property name) causes runtime `InvalidOperationException`. *Mitigation:* Verify property names in composite key expressions match the actual entity class definitions exactly.
- **Connection String Invalid:** If the connection string in `appsettings.json` doesn't match the actual SQL Server instance name or database name, `DbContext` initialization throws `SqlException`. *Mitigation:* Test the connection string manually (e.g., via SSMS) before adding to config; confirm database already exists (created by `database.sql` script).
