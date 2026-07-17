# Phase 4: Q2 Director_Movie Razor Page

## Requirements

Implement a single Razor Page at `solution/Q2_MovieApp/Pages/Movies/Director_Movie.cshtml` accessible via route `/Movies/Director_Movie` that displays all movies and directors from givenAPI. On first visit (no query string), the page loads all movies and all directors via `GivenApiClient`. When a query string `?directorId={id}` is present, the page loads only movies directed by that director (via `GetMoviesByDirectorId/{id}`). The movie table must render with exact HTML id conventions: each cell must have id `td_{columnName}_{movieId}` where columnName is in camelCase (title, releaseDate, description, language, director, stars), and each delete button must have id `btn_delete_{movieId}`. The director list in the left panel must render each director as a link with id `di_{directorId}`. Clicking a delete button triggers a handler that calls givenAPI's `DeleteMovie/{id}` endpoint and redirects back to `/Movies/Director_Movie` (full-page reload, no AJAX).

## Design Constraints

Preflight: reuses Phase 3's conventions (`Q2_MovieApp.Pages` file-scoped namespace, `GivenApiClient` injected via constructor DI). No new conventions introduced.

**Allowed exception (delete uses GET, not POST):** the spec's HTML Elements table (question_2.md §4) requires the Delete control to literally be an `<a>` tag with id `btn_delete_{movieId}` — not a `<button type=submit>` inside a form. A plain `<a href>` click is always a browser GET navigation; there is no way to make an `<a>` (without JavaScript, which is also disallowed by this phase's full-page-reload design constraint) issue a POST. So `Director_MovieModel` exposes a named handler `OnGetDeleteAsync(int id)` (invoked via `?handler=Delete&id={id}` in the `href`), not `OnPostDeleteAsync`. This is a known deviation from safe-HTTP-verb convention (a GET request has a mutating side effect), scoped narrowly to this one exam page where the exact `<a>` tag shape is graded and there's no auth/CSRF boundary to protect (givenAPI is unauthenticated and local-only).

- Must use full-page redirects and query strings (e.g., `?directorId=2`) for filtering and deletion; no AJAX, fetch, or JavaScript event handlers. This simplifies the implementation and reduces grading risk in the exam environment.
- HTML id attributes must be exactly as specified: `td_{camelCase}_{movieId}`, `btn_delete_{movieId}`, `di_{directorId}` with no variations (e.g., no hyphens, underscores only, camelCase for column names). These ids are auto-graded by the exam and typos result in point loss.
- Release Date must be formatted as `MM/dd/yyyy` (with leading zeros), differing from Q1's `M/d/yyyy` format. Use `.ToString("MM/dd/yyyy")` in Razor template.
- Stars must be joined with comma and no spaces (e.g., `"Actor1,Actor2,Actor3"`), not `"Actor1, Actor2, Actor3"`.
- The page must NOT modify the request to the Razor Pages handler to include `?handler=Delete` or similar; instead, use a direct full-page link or form that redirects after delete.
- All data loading must go through `GivenApiClient` injected into the Razor Page model; no direct HTTP calls from the view.

## Steps

1. Create `solution/Q2_MovieApp/Pages/Movies/Director_Movie.cshtml` Razor view file with `@page "/Movies/Director_Movie"` directive at the top, and skeleton HTML structure: left sidebar div for directors list, main area div for movies table, table headers (Title, Release Date, Description, Language, Director, Stars, Action), and delete action placeholders.

2. Create `solution/Q2_MovieApp/Pages/Movies/Director_Movie.cshtml.cs` PageModel class (e.g., `DirectorMovieModel`) that inherits from `PageModel`, injects `GivenApiClient` via constructor dependency injection, and declares public properties: `IEnumerable<DirectorResponse> Directors` and `IEnumerable<MovieResponse> Movies` to bind to the view.

3. Implement `OnGetAsync()` handler in the PageModel that checks for a `[BindProperty(SupportsGet = true)] int? DirectorId` property to read the query string parameter `?directorId=`. If DirectorId is null or 0, call `await givenApiClient.GetDirectorsAsync()` and `await givenApiClient.GetMoviesAsync()` to load all directors and movies; otherwise, call `await givenApiClient.GetDirectorsAsync()` (for the left panel) and `await givenApiClient.GetMoviesByDirectorIdAsync(directorId.Value)` (for the movie table), and assign results to the public properties for view binding.

4. Implement `OnPostDeleteAsync(int id)` handler that calls `await givenApiClient.DeleteMovieAsync(id)` to delete the movie from givenAPI, catches any exceptions and logs them (or silently ignores), and returns `RedirectToPage()` to reload `/Movies/Director_Movie` (the redirect does not include `?directorId=` query parameter, so the page reloads all movies; this matches the exam requirement to "redirect to the page" without specifying query string retention).

5. In the Razor view (`Director_Movie.cshtml`), render the directors list as a series of `<a>` tags in a sidebar, each with `id="di_{directorId}"` and `href="/Movies/Director_Movie?directorId={directorId}"`, displaying the director's `FullName`.

6. In the Razor view, render the movies table (`<table>`) with rows iterating over `Model.Movies`. For each movie, create six `<td>` cells with exact ids: `td_title_{movieId}`, `td_releaseDate_{movieId}`, `td_description_{movieId}`, `td_language_{movieId}`, `td_director_{movieId}`, `td_stars_{movieId}`, populated with movie data. Release Date must be formatted via `movie.ReleaseDate?.ToString("MM/dd/yyyy")` (nullable — `ReleaseDate` is `DateTime?` in givenAPI's `MovieResponse`). Stars must be joined: `string.Join(",", movie.Stars.Select(s => s.FullName))` — the star DTO's name property is `FullName` (confirmed against givenAPI's `MovieResponse.StarInfo.FullName`), not `Name`.

7. In the Razor view, add a seventh column (or button cell) with an `<a>` tag or form button with id `btn_delete_{movieId}` containing the text "Delete". This link or button must trigger the `OnPostDeleteAsync` handler; use `asp-page-handler="Delete"` with `asp-route-id="{movieId}"` to construct a POST request to the handler, or use a direct hyperlink with `href="/Movies/Director_Movie?handler=Delete&id={movieId}"` (whichever pattern the framework supports).

8. Handle potential API errors (e.g., if givenAPI is unreachable) by catching exceptions in the PageModel's `OnGetAsync` and `OnPostDeleteAsync`, logging the error, and either displaying a user-friendly error message via `TempData` or silently continuing with an empty movie list. The exam does not specify error handling behavior, so graceful degradation is acceptable.

## Success Criteria

- Navigating to `/Movies/Director_Movie` (no query string) displays a list of all directors in the left sidebar and a table of all movies from givenAPI, each with correct HTML ids (e.g., `td_title_1`, `btn_delete_1`, `di_1`).
- Release dates are formatted as `MM/dd/yyyy` (e.g., "07/16/2026").
- Stars are joined with commas and no spaces (e.g., "Tom,Jerry").
- Clicking a director link (e.g., `di_2`) navigates to `/Movies/Director_Movie?directorId=2` and the table updates to show only that director's movies.
- Clicking a delete button (e.g., `btn_delete_3`) calls givenAPI's `DeleteMovie/3` endpoint and redirects back to `/Movies/Director_Movie`, and movie with id=3 is no longer displayed in the table.
- Page renders without JavaScript errors; no console warnings or exceptions logged related to data binding or HTML rendering.

## Quality and Testing State

- Quality gate: approved (`plans/pe-trial-2/quality/phase-04-q2-movies-page-quality-report.json`, receipt issued)
- Testing: passed (60/60 unit tests across both test projects, plus full manual end-to-end verification against the real running givenAPI on port 5100: first-load table with correct HTML ids, director filter via `di_2` showing only "The Super Mario Bros. Movie" — matching the spec's own example — and a live delete of movie id=9 confirmed removed from both the page and givenAPI's store — `plans/pe-trial-2/tests/phase-04-q2-movies-page-test-report.json`)

## Risks

- **HTML ID Typos:** A single character wrong in an id attribute (e.g., `td_Title_` instead of `td_title_`, or `btn_delete` missing underscore and movie id) causes the auto-grader to fail that component. *Mitigation:* Implement ids using exact string interpolation/templates in Razor syntax; after rendering, use browser DevTools "Inspect" to verify a sample movie row contains ids like `td_title_5`, `td_releaseDate_5`, `btn_delete_5` with correct casing and underscores.
- **Release Date Format Confusion:** Using `M/d/yyyy` (Q1's format) instead of `MM/dd/yyyy` (Q2's format) fails the exam's date format check. *Mitigation:* Explicitly note in the view template that this page uses `MM/dd/yyyy`; test a known date (e.g., if a movie's release date is 2025-07-16, it must display as "07/16/2025", not "7/16/2025").
- **Stars Formatting:** If stars are joined with ", " (comma + space) instead of "," (comma only), the exam's string comparison fails. *Mitigation:* Use `string.Join(",", stars)` without a space in the join string; verify rendered output in the browser.
- **Delete Handler Routing:** If the delete button doesn't correctly route to the `OnPostDeleteAsync` handler, clicking it either navigates to a 404 or silently fails. *Mitigation:* Test by clicking a delete button and confirming the POST request is sent to the server (use browser DevTools Network tab); verify the movie disappears after the page reloads.
- **GivenAPI Unreachable:** If givenAPI is not running on port 5100, the page fails to load any data. *Mitigation:* Before testing, manually confirm givenAPI is running and accessible via `curl http://localhost:5100/api/Directors/GetDirectors` or a browser; if not, build and run it from `given_pe_trial/2/givenAPI/`.
