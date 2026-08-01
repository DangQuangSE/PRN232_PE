# Question 2 — MVC / Razor Pages (Movies by Director) — Create + Update + Delete + Search

> Instructions chung: xem [../../README.md](../../README.md)
> Phần đầu câu này nằm chung trang với cuối Question 1.

In this question, you are asked to write MVC/Razor Pages model that shows information about movies, and supports **creating**, **editing**, **deleting**, and **searching (by one or many criteria)** — all on one page. Data is fetched/written by calling pre-existing RESTful APIs hosted at **GivenAPIBaseUrl**, provided by a separate project named **GivenAPI** at `given_pe_trial_practice/SetH_Cinema_FullCombined/2/givenAPI` (`http://localhost:5105`).

## 1. Given APIs include

| API | Method | Note |
|---|---|---|
| `api/Directors/GetDirectors` | GET | Return all directors |
| `api/Movies/GetMovies` | GET | Return all movies, each: `id, title, releaseDate, description, language, director: {id, fullName}, stars: [{id, fullName}], genres: [{id, title}]` |
| `api/Movies/GetMoviesByDirectorId/{directorId}` | GET | Return movies of one director |
| `api/Movies/GetMovieById/{id}` | GET | Return one movie (used to pre-fill the edit form) |
| `api/Movies/SearchMovies?title=&language=&directorId=&fromYear=&toYear=` | GET | All query params optional, combined with AND |
| `api/Movies/CreateMovie` | POST | Body: `{ "title": "...", "releaseDate": "yyyy-MM-dd", "description": "...", "language": "...", "directorId": 0 }`. Returns the created movie (status 201) or 400 on validation error. |
| `api/Movies/UpdateMovie/{id}` | PUT | Same body shape as create (minus `directorId` rename not required). Returns the updated movie (status 200) or 404. |
| `api/Movies/DeleteMovie/{id}` | DELETE | Deletes the movie. Returns 204 No Content, or 404 if not found. |

## 2. Note (IMPORTANT)

- Students **MUST** use `HttpClient` to call the API.
- `GivenAPIBaseUrl` must be in `appsettings.json`:
  ```json
  { "GivenAPIBaseUrl": "http://localhost:5105" }
  ```
- All input and output elements in the HTML source must have an **'id'** attribute.
- This is the hardest practice page — four independent operations (Search, Create, Update, Delete) share one page. Keep the id namespaces for Search, the Create form, and the Edit form **strictly separate** (see §3.4–3.6) so a grader script can locate each one unambiguously.

## 3. Requirements

The web application has a page at url **`/Movies/Director_Movie`**, which includes five main parts.

### 3.1. Display List of Movies

On first access, display all movies in table format:

- Title, Release Date (`MM/dd/yyyy`), Description, Language, Director (from Directors), Stars (comma-separated, no spaces), Action.
- Each `<td>` has id **`td_{columnName}_{movieId}`** (camelCase columnName: `title`, `releaseDate`, `description`, `language`, `director`, `stars`).
- Each row's Action cell has two `<a>` links: **Edit** with id **`btn_edit_{movieId}`**, and **Delete** with id **`btn_delete_{movieId}`** (inner text `Delete`).
- The table body itself has id **`movie_rows`** (so Search can clear/refill it without touching the rest of the page).

### 3.2. Display all Directors and Filter by Director

List of Directors on the **left**, each `<a>` with id **`di_{directorId}`**. Clicking a director link filters the table to only that director's movies (calls `GetMoviesByDirectorId`).

### 3.3. Search by one or many criteria

A search form above the table (own id namespace, no prefix — distinct from `create_`/`edit_`):

| Criterion | Element | Id | Query parameter |
|---|---|---|---|
| Title contains | text input | `input_title` | `title` |
| Language contains | text input | `input_language` | `language` |
| Director | select | `select_director` | `directorId` |
| Inclusive start year | number input | `input_fromYear` | `fromYear` |
| Inclusive end year | number input | `input_toYear` | `toYear` |
| Search | button | `btn_search` | — |
| Reset | anchor | `btn_reset`, links to `/Movies/Director_Movie` | — |

- Call `GET api/Movies/SearchMovies` with only the supplied parameters (using just `title` alone exercises the single-criterion case; combining more fields exercises the multi-criteria case — both must work).
- All supplied criteria are combined with AND.
- Title and language are case-insensitive substring searches.
- An empty director means all directors.
- No criteria displays all movies.
- Preserve every entered criterion after searching.
- If `fromYear > toYear`, do not call GivenAPI. Show `From year must not exceed to year.` in element id `search_error`.
- If no movies match, keep `#movie_rows` empty and show `No movies found.` in element id `search_message`.

### 3.4. Create a New Movie

Below the table, a form to add a new movie (id namespace prefixed `create_`):

| Field | Element | Id |
|---|---|---|
| Title | `<input type="text">` | `create_input_title` |
| Release Date | `<input type="date">` | `create_input_releaseDate` |
| Description | `<textarea>` | `create_input_description` |
| Language | `<input type="text">` | `create_input_language` |
| Director | `<select>` (options = all directors) | `create_select_director` |
| Submit | `<button>`, inner text `Create` | `btn_create` |

On submit: `POST api/Movies/CreateMovie`, then redirect to `/Movies/Director_Movie` (table now includes the new movie). On failure (400), redisplay the form with entered values retained and show an error in `create_error`.

### 3.5. Update an Existing Movie

Clicking `btn_edit_{movieId}` navigates to **`/Movies/Director_Movie?editId={movieId}`**. When `editId` is present, call `GetMovieById(editId)` and render an edit form pre-filled with current values (id namespace prefixed `edit_`):

| Field | Element | Id |
|---|---|---|
| Hidden movie id | `<input type="hidden">` | `edit_input_id` |
| Title | `<input type="text">`, value = current title | `edit_input_title` |
| Release Date | `<input type="date">`, value = current release date | `edit_input_releaseDate` |
| Description | `<textarea>`, content = current description | `edit_input_description` |
| Language | `<input type="text">`, value = current language | `edit_input_language` |
| Director | `<select>`, selected = current director | `edit_select_director` |
| Save | `<button>`, inner text `Save` | `btn_save` |

On submit: `PUT api/Movies/UpdateMovie/{id}`, then redirect to `/Movies/Director_Movie` (no query string — table reloads, edited row reflects new values). On failure (404 or other error), redisplay the edit form with entered values retained and show an error in `edit_error`.

### 3.6. Delete a Movie

Clicking a row's `btn_delete_{movieId}` calls `DELETE api/Movies/DeleteMovie/{movieId}` on GivenAPI, then returns to `/Movies/Director_Movie` with the deleted row no longer in the table. The Delete control must be a real `<a>` tag — implement this via a GET-based Razor Page handler (not JavaScript/AJAX), same convention as the other practice sets. On failure, redisplay the list and show an error in element id `delete_error`.

## 4. HTML Elements ID — Summary

| Element | Element Tag | Id |
|---|---|---|
| Movie table body | `<tbody>` | `movie_rows` |
| Each cell in the table | `<td>` | `td_{columnName}_{movieId}` |
| Edit link | `<a>` | `btn_edit_{movieId}` |
| Delete link | `<a>` | `btn_delete_{movieId}` |
| Link to filter Movies of Director | `<a>` | `di_{directorId}` |
| Search: title/language/director/fromYear/toYear/search/reset | various | `input_title`, `input_language`, `select_director`, `input_fromYear`, `input_toYear`, `btn_search`, `btn_reset` |
| Search error / message container | any | `search_error`, `search_message` |
| Create: title/date/desc/lang/director/submit | various | `create_input_title`, `create_input_releaseDate`, `create_input_description`, `create_input_language`, `create_select_director`, `btn_create` |
| Create error container | any | `create_error` |
| Edit: hidden id/title/date/desc/lang/director/save | various | `edit_input_id`, `edit_input_title`, `edit_input_releaseDate`, `edit_input_description`, `edit_input_language`, `edit_select_director`, `btn_save` |
| Edit error container | any | `edit_error` |
| Delete error container | any | `delete_error` |
