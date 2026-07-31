# Question 2 — MVC / Razor Pages (Movies by Director) — Create

> Instructions chung: xem [../../README.md](../../README.md)
> Phần đầu câu này nằm chung trang với cuối Question 1.

In this question, you are asked to write MVC/Razor Pages model that shows information about movies, and allows **creating a new movie**. Data is fetched/written by calling pre-existing RESTful APIs hosted at **GivenAPIBaseUrl** — a separate project named **GivenAPI**, already provided and runnable at `given_pe_trial_practice/SetG_Cinema_JwtAuth/2/givenAPI` (`http://localhost:5104`). This GivenAPI has **no JWT/authentication** — the JWT requirement in Question 1 only applies to your own Q1 project, not this one.

## 1. Given APIs include

| API | Method | Note |
|---|---|---|
| `api/Directors/GetDirectors` | GET | Return all directors |
| `api/Movies/GetMovies` | GET | Return all movies, each: `id, title, releaseDate, description, language, director: {id, fullName}, stars: [{id, fullName}], genres: [{id, title}]` |
| `api/Movies/GetMoviesByDirectorId/{directorId}` | GET | Return movies of one director |
| `api/Movies/CreateMovie` | POST | Body: `{ "title": "...", "releaseDate": "yyyy-MM-dd", "description": "...", "language": "...", "directorId": 0 }`. Returns the created movie (status 201) or 400 on validation error. |

## 2. Note (IMPORTANT)

- Students **MUST** use `HttpClient` to call the API.
- `GivenAPIBaseUrl` must be in `appsettings.json`:
  ```json
  { "GivenAPIBaseUrl": "http://localhost:5104" }
  ```
- All input and output elements in the HTML source must have an **'id'** attribute.

## 3. Requirements

The web application has a page at url **`/Movies/Director_Movie`**, which includes three main parts.

### 3.1. Display List of Movies

On first access, display all movies in table format:

- Title, Release Date (`MM/dd/yyyy`), Description, Language, Director (from Directors), Stars (comma-separated, no spaces).
- Each `<td>` has id **`td_{columnName}_{movieId}`** (camelCase columnName: `title`, `releaseDate`, `description`, `language`, `director`, `stars`).

### 3.2. Display all Directors and Filter by Director

List of Directors on the **left**, each `<a>` with id **`di_{directorId}`**. Clicking a director link filters the table to only that director's movies (calls `GetMoviesByDirectorId`).

### 3.3. Create a New Movie (main operation of this question)

Below the table, a form to add a new movie:

| Field | Element | Id |
|---|---|---|
| Title | `<input type="text">` | `input_title` |
| Release Date | `<input type="date">` | `input_releaseDate` |
| Description | `<textarea>` | `input_description` |
| Language | `<input type="text">` | `input_language` |
| Director | `<select>` (options = all directors, value = director id, text = director full name) | `select_director` |
| Submit | `<button>` or `<input type="submit">`, inner text `Create` | `btn_create` |

Behavior:
- On submit, call `POST api/Movies/CreateMovie` on GivenAPI with the form values.
- On success, redirect back to `/Movies/Director_Movie` (no query string — table reloads showing all movies, including the newly created one, with correct `td_title_{newId}` etc.).
- On failure (GivenAPI returns 400), redisplay the form with the values the user entered still filled in, and show an error message in an element with id `create_error` (no fixed error text required, but the element must exist and be non-empty when there is an error).

## 4. HTML Elements ID — Summary

| Element | Element Tag | Id |
|---|---|---|
| Each cell in the table | `<td>` | `td_{columnName}_{movieId}` |
| Link to filter Movies of Director | `<a>` | `di_{directorId}` |
| Title input | `<input>` | `input_title` |
| Release Date input | `<input>` | `input_releaseDate` |
| Description input | `<textarea>` | `input_description` |
| Language input | `<input>` | `input_language` |
| Director select | `<select>` | `select_director` |
| Create button | `<button>` | `btn_create` |
| Error message container | any | `create_error` |
