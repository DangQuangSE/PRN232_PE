# Phase 2: Q1 Director Endpoints

## Requirements

Implement three RESTful endpoints in Q1_WebAPI that handle director queries and creation: (1) `GET /api/director/getdirectors/{nationality}/{gender}` filters directors by nationality (case-insensitive) and gender (string "male"/"female" converted to bool), returning director DTOs with a `dobString` property formatted as `M/d/yyyy` (no leading zeros); (2) `GET /api/director/getdirector/{id}` returns a single director with an array of their directed movies (genres and stars arrays are empty, not populated); (3) `POST /api/director/create` accepts a JSON body with fullName, male, dob, nationality, description, inserts a new director record, and returns HTTP 200 with count=1 if successful, or HTTP 409 Conflict with plain-text message `"There is an error while adding."` if a database exception occurs.

## Design Constraints

- All endpoints must use the AppDbContext configured in Phase 1; no direct SQL or stored procedures.
- The `dobString` property must use .NET's `ToString("M/d/yyyy")` format (e.g., "4/9/1975", not "04/09/1975"); this format is tested by the exam grader and differs from Q2's `MM/dd/yyyy` format.
- Gender filtering must accept string values "male" or "female" (case-insensitive) and map them to boolean comparisons against the `Gender` (bool) field in the database; "male" → true, "female" → false.
- The `POST /api/director/create` endpoint must wrap `SaveChanges()` in a try/catch block targeting `DbUpdateException` (or broader `Exception` if needed) and return exactly the string `"There is an error while adding."` as the response body (not a JSON object); status code must be 409 Conflict.
- No authentication, authorization, or CORS-specific headers are required (CORS is out of scope per the exam).
- Endpoints must return JSON by default; use DTOs or anonymous objects to shape responses, not raw entity objects.

## Steps

1. Create a `solution/Q1_WebAPI/Models/DTOs/DirectorDto.cs` class with fields matching the expected response: `id`, `fullName`, `gender`, `dob`, `dobString`, `nationality`, `description`. Include a constructor or mapping method to convert from a Director entity to DirectorDto, computing `dobString` via `dob.ToString("M/d/yyyy")`.

2. Create a `solution/Q1_WebAPI/Models/DTOs/DirectorWithMoviesDto.cs` class with all DirectorDto fields plus a `movies` array property. **Per spec.md's example response for 1.2, each movie object must include exactly:** `id`, `title`, `releaseDate`, `releaseYear` (int, computed as `movie.ReleaseDate?.Year`), `description`, `language`, `producerId`, `directorId`, `producerName` (from `movie.Producer?.Name`, requires the Phase 1 `Movie.Producer` navigation), `directorName` (from `movie.Director?.FullName`), plus empty `genres: []` and `stars: []` arrays (fixed empty lists per spec — do NOT query Movie_Genre/Movie_Star for this endpoint).

3. Create a `solution/Q1_WebAPI/Models/DTOs/CreateDirectorRequest.cs` class with properties `fullName`, `male` (bool), `dob` (DateTime), `nationality`, `description` to bind incoming POST request bodies.

4. Create `solution/Q1_WebAPI/Controllers/DirectorController.cs` with `[ApiController]` and `[Route("api/director")]` attributes; inject the AppDbContext via constructor dependency injection.

5. Implement `GetDirectors(string nationality, string gender)` endpoint at `[HttpGet("getdirectors/{nationality}/{gender}")]` that filters the Directors DbSet by nationality (case-insensitive comparison using `.Contains()` or `.Equals(StringComparison.OrdinalIgnoreCase)`), parses the gender string to a boolean (e.g., "male".Equals("male", OrdinalIgnoreCase) → true), filters by gender, maps results to DirectorDto array with `dobString` computed, and returns `Ok(directors)`.

6. Implement `GetDirector(int id)` endpoint at `[HttpGet("getdirector/{id}")]` that queries for the director by id, includes related movies via `.Include(d => d.Movies).ThenInclude(m => m.Producer)` (Producer needed for `producerName`), maps to DirectorWithMoviesDto with `producerName`/`directorName`/`releaseYear` populated and `genres`/`stars` forced to empty arrays for each movie, and returns `Ok(directorWithMovies)` if found or `NotFound()` if the director does not exist.

7. Implement `CreateDirector(CreateDirectorRequest request)` endpoint at `[HttpPost("create")]` that wraps the database operation in a try/catch block: inside try, create a new Director entity from the request, add to `context.Directors`, call `SaveChanges()`, and return `Ok(1)` (representing 1 record added); inside catch (catching `DbUpdateException` or `Exception`), return a **409 status with a literal plain-text body** (not JSON-quoted) using a `ContentResult`:
   ```csharp
   catch (DbUpdateException)
   {
       return new ContentResult
       {
           StatusCode = StatusCodes.Status409Conflict,
           Content = "There is an error while adding.",
           ContentType = "text/plain"
       };
   }
   ```
   Using `Conflict("...")` instead would return the string JSON-encoded (wrapped in escaped quotes) via the default `[ApiController]` content negotiation — avoid that, since the spec's example shows the raw message with no quotes.

8. Register the DirectorController in `Program.cs` via `builder.Services.AddControllers()` if not already done; ensure routing is configured to recognize `[Route]` attributes.

9. Test the 409 path directly: stop SQL Server (or point the connection string at a non-existent database temporarily) and run `curl -i -X POST http://localhost:5000/api/director/create -H "Content-Type: application/json" -d "{\"fullName\":\"Test\",\"male\":true,\"dob\":\"2000-01-01\",\"nationality\":\"usa\",\"description\":\"x\"}"`; confirm the response is `HTTP/1.1 409` with body exactly `There is an error while adding.` (no surrounding quotes).

## Success Criteria

- `GET /api/director/getdirectors/usa/male` returns a JSON array of directors with nationality "usa" (case-insensitive match) and gender true (male), each with correct `dobString` format "M/d/yyyy" (e.g., "4/9/1975").
- `GET /api/director/getdirector/5` returns a single director object with all fields plus a `movies` array containing that director's movies, each movie including `producerName`/`directorName`/`releaseYear` and empty `genres`/`stars`; if director id=5 does not exist, returns HTTP 404 NotFound.
- `POST /api/director/create` with a valid JSON body `{fullName: "Test", male: true, dob: "2000-01-01T00:00:00", nationality: "usa", description: "test"}` returns HTTP 200 with response body `1`.
- `POST /api/director/create` when the SQL Server is unreachable or SaveChanges() throws an exception returns HTTP 409 Conflict with response body string `"There is an error while adding."`.
- All three endpoints are accessible at their exact paths without redirection or additional routing configuration.

## Quality and Testing State

- Quality gate: approved (`plans/pe-trial-2/quality/phase-02-q1-director-endpoints-quality-report.json`, receipt issued)
- Testing: passed (38/38 unit tests incl. Phase 1's, plus manual curl verification of all 3 endpoints against the real SQL Server DB — `plans/pe-trial-2/tests/phase-02-q1-director-endpoints-test-report.json`)

## Risks

- **dobString Format Mismatch:** Using `M/d/yyyy` is correct for Q1, but mixing it up with Q2's `MM/dd/yyyy` is an easy error. *Mitigation:* Verify the format by checking a known example, e.g., if a director's dob is 1975-04-09, dobString must output "4/9/1975" (not "04/09/1975").
- **Gender String Parsing Logic:** Incorrect case-sensitive comparison or reversed boolean logic (e.g., "male" → false) causes filter to return wrong directors. *Mitigation:* Test `GetDirectors` with both "male" and "female" parameters; verify returned directors have correct gender values in the database.
- **409 Conflict Response Format:** Returning a JSON object like `{"error": "..."}`, or using `Conflict("...")` (which JSON-encodes the string with escaped quotes under `[ApiController]`'s default content negotiation), fails the exam grader if it expects the raw message. *Mitigation:* Use the `ContentResult` pattern in Step 7 (`ContentType = "text/plain"`, `Content = "There is an error while adding."`) and confirm via the Step 9 curl test that the body has no surrounding quotes.
- **Missing Include() for Movies:** If `GetDirector` queries without `.Include(d => d.Movies)`, the movies navigation property is null or lazy-loaded incorrectly, returning an empty array instead of the director's actual movies. *Mitigation:* Explicitly use `.Include()` in the LINQ query to eagerly load related movies before returning.
