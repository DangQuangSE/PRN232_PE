# Plan: PE Trial (Paper No. 2) — Question 1 Web API + Question 2 Razor Pages

Status: 🟢 Completed
Date: 2026-07-16
Mode: Hard

## Overview

Build two independent ASP.NET Core 8 projects from scratch for a practical exam: (1) a Web API (Q1) that exposes director/movie CRUD operations against a fixed SQL Server schema via manual EF Core DbContext (Database-First, not scaffolded), and (2) a Razor Pages app (Q2) that calls an existing givenAPI to display, filter, and delete movies with exact HTML id conventions. Both projects must match specification requirements precisely for grading automation.

## Phases

- [x] Phase 1: Q1 Project Scaffold + EF Core Models & DbContext — Create ASP.NET Core 8 Web API project scaffold, define entity models (Director, Movie, Producer, Star, Genre, Movie_Genre, Movie_Star with composite keys), build manual EF Core DbContext with Fluent API mapping to match database.sql schema, wire connection string from appsettings.
- [x] Phase 2: Q1 Director Endpoints — Implement three director endpoints (getdirectors with nationality/gender filtering, getdirector with movies + empty genres/stars arrays, create with 409 Conflict error handling).
- [x] Phase 3: Q2 Project Scaffold + HttpClient Service — Create Razor Pages project scaffold, configure IHttpClientFactory, build GivenApiClient service layer to call givenAPI endpoints, wire GivenAPIBaseUrl from appsettings.
- [x] Phase 4: Q2 Director_Movie Razor Page — Implement Director_Movie.cshtml page at /Movies/Director_Movie route, load all movies + directors on first visit, support directorId query string filtering, delete movies with exact HTML id conventions (td_{camelCase}_{movieId}, btn_delete_{movieId}, di_{directorId}).

## Research Summary

### Research Findings & Chosen Approach

The brainstorm confirmed the following decisions:

1. **Database-First over Code-First:** The exam provides a fixed SQL Server schema via `given_pe_trial/1/database.sql`. Rather than scaffolding (which might bloat the solution) or using code-first migrations (which would re-create the schema), we manually write entity classes and Fluent API mappings to match the exact schema. This avoids tooling risks during exam grading and gives full control over DbContext structure.

2. **Composite Key Fluent API Patterns:** The Movie_Genre and Movie_Star junction tables use composite primary keys (MovieId + GenreId, MovieId + StarId). Research confirmed Fluent API `.HasKey(e => new { e.MovieId, e.GenreId })` is the standard pattern; include this as direct implementation guidance in Phase 1.

3. **dobString Format Clarity:** The spec distinguishes between Q1's director `dobString` format (`M/d/yyyy` — no leading zeros, e.g., "4/9/1975") and Q2's movie `releaseDate` format (`MM/dd/yyyy` — with leading zeros, e.g., "07/16/2026"). Research confirmed .NET's `ToString("M/d/yyyy")` vs `ToString("MM/dd/yyyy")` produce exactly this difference. Phase 1 and Phase 4 must use their respective formats.

4. **409 Conflict on DB Insert Errors:** Research validated that Q1's `POST /api/director/create` must catch `DbUpdateException` (thrown by EF Core when SaveChanges() fails) and return HTTP 409 Conflict with the exact error message string `"There is an error while adding."` (not a JSON object, just a plain string). Phase 2 includes this as concrete implementation guidance.

5. **Minimal NuGet Packages:** Confirmed that for EF Core + SQL Server, only `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Design` are needed (the latter for tooling support). No extra packages like Automapper, Serilog, or identity frameworks are expected or permitted in an exam context.

6. **Razor Pages + Full-Page Reload for Q2:** The spec does not mandate AJAX; using full-page redirects with query strings (`?directorId=`) simplifies implementation and removes JS-related grading risk. This is a deliberate architectural choice—not a limitation—that matches the exam's pragmatic intent.

7. **GivenAPI as Read-Only Reference:** Q2 must not modify `given_pe_trial/2/givenAPI`. Its endpoints (`GetDirectors`, `GetMovies`, `GetMoviesByDirectorId/{id}`, `DeleteMovie/{id}`) are already implemented and running on port 5100. Q2's responsibility is only to consume these endpoints correctly via HttpClient and render the response with exact HTML ids.

### Why This Approach Wins

- **Exam-Safe:** Manual DbContext + Fluent API gives us full control without relying on scaffolding tools (which could produce unexpected code or fail in exam environments).
- **Precision:** We know exactly what query patterns EF Core will generate; no surprises.
- **Grading-Proof:** HTML ids and response formats are under our control; easier to ensure they match the auto-grader's expectations.
- **No Time Waste:** Database-First with manual mapping is faster than debugging scaffolding or fighting migrations during an exam.

## Dependencies

1. **SQL Server Instance:** Must be running locally (LocalDB or SQL Server Express) with `given_pe_trial/1/database.sql` already executed before Q1 can be tested end-to-end. This is an environment precondition set by the exam; the plan does not handle SQL Server installation/setup.
2. **GivenAPI Running:** Q2 requires the existing `given_pe_trial/2/givenAPI` project to be built and running on `http://localhost:5100` during Q2 development and testing.
3. **ASP.NET Core 8 SDK:** Both projects target .NET 8; SDK must be installed locally.

## Risks

- **HIGH: Database Schema Mismatch** — If the manual EF Core entity definitions don't exactly match the columns/types in `database.sql`, queries will fail or return wrong data. *Mitigation:* Phase 1 must carefully cross-check each entity property against the actual SQL script line-by-line; run a sample query against the real DB immediately after DbContext is written to validate mappings.

- **HIGH: HTML ID Conventions Grading Failure** — Q2's exact HTML ids are auto-graded; a single typo (e.g., `td_Title_` instead of `td_title_`, or `btn_delete` instead of `btn_delete_`) causes point loss. *Mitigation:* Phase 4 must implement IDs using exact string templates in the Razor view; add a manual spot-check step to verify a rendered HTML page contains all required ids before signing off.

- **HIGH: dobString/releaseDate Format Confusion** — Mixing up `M/d/yyyy` (Q1) and `MM/dd/yyyy` (Q2) is an easy mistake. *Mitigation:* Phase 2 and Phase 4 each explicitly call out their format; consider adding a unit test or integration test verifying the exact format (e.g., "4/9/1975" vs "07/16/2026").

- **MEDIUM: GivenAPI Port/URL Configuration** — If Q2's appsettings.json has the wrong `GivenAPIBaseUrl` (e.g., `localhost:5100` vs `127.0.0.1:5100` or typo in endpoint path), calls to givenAPI will fail silently or throw exceptions. *Mitigation:* Phase 3 must document the exact URL format; Phase 4's initial load should log or display API errors if the connection fails.

- **MEDIUM: Composite Key Fluent API Typos** — Writing `.HasKey(e => new { e.MovieId, e.GenreId })` is straightforward, but a typo in property names (e.g., `MovieGenreId` instead of `MovieId`) causes runtime errors. *Mitigation:* Phase 1 must test DbContext creation and query operations immediately after writing Fluent configurations; use `context.Database.EnsureCreated()` or a simple LINQ query to trigger lazy loading.

- **LOW: HTTP Status Codes Confusion** — Q1's `getdirector/{id}` must return 404 if id not found; `create` must return 409 on DB errors. A misplaced error handler or incorrect status code is caught by Phase 2's testing. *Mitigation:* Phase 2 includes concrete steps to verify each endpoint's status code behavior; use `curl` or Postman to test edge cases before integration.

- **LOW: CORS and Cross-Origin Requests** — Q2 calls Q1 API from the browser if hosted together; Q2 calls givenAPI from the server-side HttpClient (no browser CORS issue). *Mitigation:* If Q1 and Q2 run on different domains in the future, add CORS to Q1; for now, server-to-server calls are unaffected.

- **NOTED (plan-review): Q2 Port Conflict** — Phase 1 pins Q1 to port 5000; Phase 3 doesn't pin a port for Q2, so it may default to 5000 too and collide if both run simultaneously. Since only givenAPI (5100) needs to be running for Q2 to work, and Q1/Q2 are graded/run independently per the exam's separate-submission model, this is unlikely to matter in practice — but Phase 3 should still set an explicit, different port (e.g. 5001) in `launchSettings.json` to avoid confusion during local dev.

- **NOTED (plan-review): JSON Casing Assumption** — The plan relies on ASP.NET Core 8's default camelCase JSON serialization to produce `dobString`, `fullName`, etc. from PascalCase C# DTO properties. This is the framework default and requires no extra config, but Phase 2's success criteria should include an explicit `curl` check of a live response to confirm casing before considering the phase done.

- **NOTED (plan-review): No Input Validation on POST /create** — Phase 2 does not validate `CreateDirectorRequest` fields (empty fullName, invalid dob, etc.). The spec doesn't require it and the exam's grading examples only cover the happy path and the DB-exception path, so this is out of scope by design, not an oversight.

---

## Technical Notes for Implementer

1. **Connection String Format:** The database name must be exactly `PE_PRN_Fall22B1` (created by `given_pe_trial/1/database.sql`'s `CREATE DATABASE [PE_PRN_Fall22B1]`) — not a placeholder. The user's local SQL Server uses **SQL Server Authentication** (`sa` / `12345`), so: `"Server=localhost;Database=PE_PRN_Fall22B1;User Id=sa;Password=12345;TrustServerCertificate=true;"`. If `localhost` doesn't resolve to the right instance, try `.` or the named instance (e.g. `.\SQLEXPRESS`); Mixed Mode authentication must be enabled on the instance for `sa` login to work.

2. **Shared Solution File:** Both projects live under one `solution/PE_Trial.sln` (created in Phase 1, `Q2_MovieApp` added in Phase 3) so they open together in Visual Studio. This doesn't change the separate-submission requirement — each question is still published independently via `dotnet publish -c Release -o ./[QuestionNumber_StudentAccount] --project solution/Q1_WebAPI` (and similarly for Q2).

3. **Fluent API Composite Keys Example Pattern:**
   ```csharp
   // In DbContext OnModelCreating:
   modelBuilder.Entity<MovieGenre>()
       .HasKey(e => new { e.MovieId, e.GenreId });
   ```
   Same pattern for Movie_Star.

4. **dobString Conversion Example:**
   ```csharp
   // In entity or DTO:
   public string DobString => Dob.ToString("M/d/yyyy");
   ```

5. **409 Conflict Error Handling Example Pattern** (plain-text body, not JSON-encoded — see Phase 2 Step 7 for why `Conflict("...")` is the wrong choice here):
   ```csharp
   try {
       db.Directors.Add(new Director { ... });
       db.SaveChanges();
       return Ok(1);
   } catch (DbUpdateException) {
       return new ContentResult {
           StatusCode = StatusCodes.Status409Conflict,
           Content = "There is an error while adding.",
           ContentType = "text/plain"
       };
   }
   ```

6. **GivenAPI Consumption Example Pattern:**
   ```csharp
   // In GivenApiClient:
   var response = await httpClient.GetAsync($"{baseUrl}/api/Movies/GetMovies");
   var json = await response.Content.ReadAsStringAsync();
   return JsonSerializer.Deserialize<List<MovieResponse>>(json,
       new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
   ```

7. **HTML ID String Templates (Q2):**
   ```html
   <td id="td_title_@movie.Id">@movie.Title</td>
   <a id="btn_delete_@movie.Id" href="/Movies/Director_Movie?handler=Delete&id=@movie.Id">Delete</a>
   <a id="di_@director.Id" href="/Movies/Director_Movie?directorId=@director.Id">@director.FullName</a>
   ```
   (Exact Razor syntax will be refined in Phase 4; note the earlier draft had a stray `}` after `@movie.Id` in the delete link id — removed here.)
