# Question 2 — MVC / Razor Pages (Books by Author) — Create

> Instructions chung: xem [../../README.md](../../README.md)
> Phần đầu câu này nằm chung trang với cuối Question 1.

In this question, you are asked to write MVC/Razor Pages model that shows information about books, and allows **creating a new book**. The application fetches/writes data by calling pre-existing RESTful APIs hosted at **GivenAPIBaseUrl**, provided by a separate project named **GivenAPI** (see the note in [../../README.md](../../README.md) about this practice set not shipping a runnable GivenAPI — you must stand one up yourself matching the contract below before testing this page end-to-end).

## 1. Given APIs include

| API | Method | Note |
|---|---|---|
| `api/Authors/GetAuthors` | GET | Return all authors |
| `api/Books/GetBooks` | GET | Return all books (with nested author/translators/genres, same shape as Question 1.2's book object minus publisherName/directorName-style extras — just `id, title, publishDate, description, language, author: {id, fullName}, translators: [{id, fullName}], genres: [{id, title}]`) |
| `api/Books/GetBooksByAuthorId/{authorId}` | GET | Return books of one author |
| `api/Books/CreateBook` | POST | Body: `{ "title": "...", "publishDate": "yyyy-MM-dd", "description": "...", "language": "...", "authorId": 0 }`. Returns the created book (status 201) or 400 on validation error. |

## 2. Note (IMPORTANT)

- Students **MUST** use `HttpClient` to call the API.
- `GivenAPIBaseUrl` must be in `appsettings.json`:
  ```json
  { "GivenAPIBaseUrl": "http://localhost:5100" }
  ```
- All input and output elements in the HTML source must have an **'id'** attribute.

## 3. Requirements

The web application has a page at url **`/Books/Author_Book`**, which includes three main parts.

### 3.1. Display List of Books

On first access, display all books in table format:

- Title, Publish Date (`MM/dd/yyyy`), Description, Language, Author (from Authors), Translators (comma-separated, no spaces).
- Each `<td>` has id **`td_{columnName}_{bookId}`** (camelCase columnName: `title`, `publishDate`, `description`, `language`, `author`, `translators`).

### 3.2. Display all Authors and Filter by Author

List of Authors on the **left**, each `<a>` with id **`di_{authorId}`**. Clicking an author link filters the table to only that author's books (calls `GetBooksByAuthorId`).

### 3.3. Create a New Book (main operation of this question)

Below the table, a form to add a new book:

| Field | Element | Id |
|---|---|---|
| Title | `<input type="text">` | `input_title` |
| Publish Date | `<input type="date">` | `input_publishDate` |
| Description | `<textarea>` | `input_description` |
| Language | `<input type="text">` | `input_language` |
| Author | `<select>` (options = all authors, value = author id, text = author full name) | `select_author` |
| Submit | `<button>` or `<input type="submit">`, inner text `Create` | `btn_create` |

Behavior:
- On submit, call `POST api/Books/CreateBook` on GivenAPI with the form values.
- On success, redirect back to `/Books/Author_Book` (no query string — table reloads showing all books, including the newly created one, with correct `td_title_{newId}` etc.).
- On failure (GivenAPI returns 400), redisplay the form with the values the user entered still filled in, and show an error message in an element with id `create_error` (no fixed error text required, but the element must exist and be non-empty when there is an error).

## 4. HTML Elements ID — Summary

| Element | Element Tag | Id |
|---|---|---|
| Each cell in the table | `<td>` | `td_{columnName}_{bookId}` |
| Link to filter Books of Author | `<a>` | `di_{authorId}` |
| Title input | `<input>` | `input_title` |
| Publish Date input | `<input>` | `input_publishDate` |
| Description input | `<textarea>` | `input_description` |
| Language input | `<input>` | `input_language` |
| Author select | `<select>` | `select_author` |
| Create button | `<button>` | `btn_create` |
| Error message container | any | `create_error` |
