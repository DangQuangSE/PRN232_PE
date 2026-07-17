# Phase 3: Q2 Project Scaffold & HttpClient Service Layer

## Requirements

Create a new ASP.NET Core 8 Razor Pages project at `solution/Q2_MovieApp`, configure `IHttpClientFactory` for outbound HTTP calls, wire the base URL for the existing `givenAPI` (running on `http://localhost:5100`) from `appsettings.json` under key `GivenAPIBaseUrl`, and implement a `GivenApiClient` service class that encapsulates HTTP calls to givenAPI's endpoints (GetDirectors, GetMovies, GetMoviesByDirectorId/{id}, DeleteMovie/{id}), deserializing responses into properly typed response classes matching givenAPI's contract (examine `given_pe_trial/2/givenAPI/givenAPI/Models/MovieResponse.cs` to understand the exact field names and structure).

## Design Constraints

- Must not modify any code in `given_pe_trial/2/givenAPI`; givenAPI is a read-only reference project owned by the exam.
- Must use `IHttpClientFactory` for creating `HttpClient` instances (not `HttpClient` as a singleton or direct instantiation); this is the modern ASP.NET Core best practice and ensures proper resource management.
- All HTTP calls to givenAPI must go through the `GivenApiClient` service; no direct `HttpClient` calls from Razor Pages code paths or other layers.
- The `GivenAPIBaseUrl` in `appsettings.json` must not include a trailing slash (e.g., `"http://localhost:5100"`), and the service must construct full endpoint URLs by concatenating base URL + relative paths (e.g., `baseUrl + "/api/Movies/GetMovies"`).
- No authentication headers or custom authorization logic is required; givenAPI is unsecured and accessible directly.
- Response deserialization must use `System.Text.Json` (the default .NET 8 JSON serializer); no external libraries like Newtonsoft.Json or Automapper.

## Steps

1. Create a new ASP.NET Core 8 Razor Pages project at `solution/Q2_MovieApp` (`dotnet new webapp -o solution/Q2_MovieApp`) with `Pages/`, `appsettings.json`, `appsettings.Development.json`, `Program.cs`, and default Razor Pages layout and home page, then add it to the shared solution created in Phase 1 (`dotnet sln solution/PE_Trial.sln add solution/Q2_MovieApp/Q2_MovieApp.csproj`). Set an explicit port in `launchSettings.json` different from Q1 (which uses 5000) and givenAPI (5100) — e.g. `5001` — to avoid a port collision if both projects are ever run side by side during local development.

2. Update `appsettings.json` to add a top-level key `"GivenAPIBaseUrl": "http://localhost:5100"` (or appropriate port/host if different).

3. In `Program.cs`, register `IHttpClientFactory` via `builder.Services.AddHttpClient()` to enable dependency injection of `IHttpClientFactory` into services.

4. **Actual givenAPI response structure (already verified against source, no further examination needed):**
   - `GET /api/Directors/GetDirectors` returns the raw `Director` model (`given_pe_trial/2/givenAPI/givenAPI/Models/Director.cs`) as a JSON array: `{ id, fullName, male, dob, nationality, description }` (camelCase on the wire; a `movies` navigation collection exists in C# but is not populated by this endpoint).
   - `GET /api/Movies/GetMovies` and `GET /api/Movies/GetMoviesByDirectorId/{id}` both return `List<MovieResponse>` (`given_pe_trial/2/givenAPI/givenAPI/Models/Responses/MovieResponse.cs`), each item shaped as:
     ```json
     {
       "id": 0, "title": "", "releaseDate": "2022-10-14T00:00:00", "description": "",
       "language": "", "producerId": 0, "directorId": 0,
       "director": { "id": 0, "fullName": "", "male": true, "dob": "...", "nationality": "", "description": "" },
       "stars": [ { "id": 0, "fullName": "", "male": true, "dob": "...", "description": "", "nationality": "" } ],
       "genres": [ { "id": 0, "title": "" } ]
     }
     ```
     **Important:** the star object's name field is `fullName`, not `name` — matching `MovieResponse.StarInfo.FullName` in givenAPI's source. Phase 4 must use `FullName` when joining star names.
   - `DELETE /api/Movies/DeleteMovie/{id}` returns **HTTP 204 No Content** on success (see `MoviesController.cs` line 170: `return NoContent();`) and HTTP 404 with a JSON body `{ "message": "..." }` if the movie doesn't exist. The client must treat 204 as success and must NOT attempt to deserialize a response body from it.

5. Create response DTOs in `solution/Q2_MovieApp/Models/` folder matching the structure above: `MovieResponse.cs` (with nested `DirectorInfo`, `StarInfo` — property `FullName`, `GenreInfo` classes, mirroring givenAPI's nesting), `DirectorResponse.cs` (flat: `Id, FullName, Male, Dob, Nationality, Description`). `System.Text.Json` deserializes camelCase JSON into PascalCase C# properties by default when the client uses case-insensitive property matching (`JsonSerializerOptions { PropertyNameCaseInsensitive = true }` — set this explicitly when deserializing in `GivenApiClient`).

6. Create `solution/Q2_MovieApp/Services/GivenApiClient.cs` that takes `IHttpClientFactory` and the base URL (from configuration) in its constructor, and exposes async methods: `GetDirectorsAsync()` (calls `/api/Directors/GetDirectors`), `GetMoviesAsync()` (calls `/api/Movies/GetMovies`), `GetMoviesByDirectorIdAsync(int directorId)` (calls `/api/Movies/GetMoviesByDirectorId/{id}`), and `DeleteMovieAsync(int movieId)` (calls `DELETE /api/Movies/DeleteMovie/{id}`, using `httpClient.DeleteAsync(...)` and only checking `response.IsSuccessStatusCode` — do NOT read/deserialize the response body, since a successful delete returns 204 No Content with an empty body). Each of the GET methods uses `HttpClient` from the factory to make the call, awaits the response, deserializes JSON into the corresponding DTO (with `PropertyNameCaseInsensitive = true`), and returns it or throws an exception if the request fails.

7. Register `GivenApiClient` in `Program.cs` dependency injection container as a scoped or transient service via `builder.Services.AddScoped<GivenApiClient>()`.

8. Create `solution/Q2_MovieApp/Pages/Movies/` folder (creating the Movies folder if it doesn't exist).

## Success Criteria

- Q2_MovieApp project builds without errors and contains `Models/`, `Services/`, and `Pages/Movies/` folders.
- `appsettings.json` includes `"GivenAPIBaseUrl": "http://localhost:5100"` (or verified correct host/port).
- `GivenApiClient` service is registered in `Program.cs` and can be injected into Razor Pages or other services.
- A manual test confirms `GivenApiClient.GetDirectorsAsync()` successfully calls givenAPI and returns a non-empty list of directors (with correct field names matching givenAPI's response).
- Response DTO classes (MovieResponse, DirectorResponse, etc.) have properties that match givenAPI's actual response structure.

## Quality and Testing State

- Quality gate: approved (`plans/pe-trial-2/quality/phase-03-q2-scaffold-and-httpclient-quality-report.json`, receipt issued; fixed 2 MEDIUM findings — missing CancellationToken propagation and undisposed HttpResponseMessage — before approval)
- Testing: passed (18/18 unit tests with stubbed HttpMessageHandler, plus manual verification against the real running givenAPI on port 5100 via a temporary /apitest endpoint — `plans/pe-trial-2/tests/phase-03-q2-scaffold-and-httpclient-test-report.json`)

## Risks

- **GivenAPI Running and Accessible:** If `given_pe_trial/2/givenAPI` is not built and running on port 5100, or if the port is blocked by firewall, `GivenApiClient` calls throw `HttpRequestException`. *Mitigation:* Before starting Phase 4, verify givenAPI is running by manually curling `http://localhost:5100/api/Directors/GetDirectors` or accessing it in a browser; if not, build and run givenAPI first.
- **Response DTO Mismatch:** If the response DTO properties don't exactly match givenAPI's JSON response (e.g., property name case mismatch, missing nested arrays), deserialization silently ignores mismatched fields or throws `JsonException`. *Mitigation:* Examine the actual JSON response from givenAPI (via Postman or curl) and ensure all field names and types in the DTO match exactly; use `System.Text.Json.JsonPropertyName` attribute if givenAPI uses camelCase JSON while C# uses PascalCase.
- **HttpClient Configuration Error:** Forgetting to register `AddHttpClient()` or incorrectly injecting `IHttpClientFactory` causes a DI resolution exception. *Mitigation:* Verify `Program.cs` includes `builder.Services.AddHttpClient()` and that `GivenApiClient` constructor accepts `IHttpClientFactory` as a parameter.
