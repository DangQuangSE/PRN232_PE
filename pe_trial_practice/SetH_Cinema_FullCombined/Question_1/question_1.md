# Question 1 — Web API (Directors) — Full CRUD + Search

> Instructions chung: xem [../../README.md](../../README.md)

Chạy script [`database.sql`](../../../given_pe_trial_practice/SetH_Cinema_FullCombined/1/database.sql) (thư mục given) trên SQL Server trước khi làm bài (tạo database `PE_Practice_CinemaH` kèm dữ liệu mẫu).

**Bảng dữ liệu:**

| Table | Columns (Data Type, Allow Null) |
|---|---|
| **Directors** | Id (int, PK) · FullName (varchar(40)) · Male (bit) · Dob (date) · Nationality (varchar(30)) · Description (ntext) |
| **Movies** | Id (int, PK) · Title (varchar(200)) · ReleaseDate (date, null) · Description (text, null) · Language (varchar(30)) · ProducerId (int, null) · DirectorId (int, null) |
| **Producers** | Id (int, PK) · Name (varchar(100)) |
| **Stars** | Id (int, PK) · FullName (varchar(100)) · Male (bit, null) · Dob (date, null) · Description (text, null) · Nationality (varchar(30), null) |
| **Genres** | Id (int, PK) · Title (nchar(10)) |
| **Movie_Genre** | MovieId (int, PK/FK) · GenreId (int, PK/FK) |
| **Movie_Star** | MovieId (int, PK/FK) · StarId (int, PK/FK) |

**Quan hệ:** Movies n—1 Directors; Movies n—1 Producers; Movies 1—n Movie_Genre n—1 Genres; Movies 1—n Movie_Star n—1 Stars.

**Note that you must:**
- using the root path `http://localhost:5000`
- using database connection string in `appsettings.json`:
  ```json
  { "ConnectionStrings": { "MyCnn": "" } }
  ```

Bài này gộp **6 mục** (thay vì 3 như các set khác) — mỗi mục lấy từ một set luyện tập khác nhau (xem bảng ánh xạ trong [README](../README.md)).

## 1.1

The API at url **`/api/director/getdirectors/{nationality}/{gender}`**:
- Receives two strings `{nationality}` and `{gender}`, using **GET**.
- Returns all directors of given nationality and gender.
- `dobString` returned with format **M/d/yyyy**.

Ví dụ (`GET /api/director/getdirectors/england/male`):
```json
[
  {
    "id": 1,
    "fullName": "Christopher Nolan",
    "gender": "Male",
    "dob": "1970-07-30T00:00:00",
    "dobString": "7/30/1970",
    "nationality": "England",
    "description": "British-American filmmaker known for Inception and The Dark Knight trilogy."
  }
]
```

## 1.2

The API at url **`/api/director/getdirector/{id}`**:
- Receives an integer `{id}`, using **GET**.
- Returns full director info including a list of movies they directed.
- `dobString` format **M/d/yyyy**.
- `genres` and `stars` return **empty list** for every movie.
- Returns **404** if the director does not exist.

Ví dụ (`GET /api/director/getdirector/1`):
```json
{
  "id": 1,
  "fullName": "Christopher Nolan",
  "gender": "Male",
  "dob": "1970-07-30T00:00:00",
  "dobString": "7/30/1970",
  "nationality": "England",
  "description": "British-American filmmaker known for Inception and The Dark Knight trilogy.",
  "movies": [
    {
      "id": 1,
      "title": "Inception",
      "releaseDate": "2010-07-16T00:00:00",
      "releaseYear": 2010,
      "description": "A thief who steals corporate secrets through dream-sharing technology.",
      "language": "English",
      "producerId": 1,
      "directorId": 1,
      "producerName": "Warner Bros. Pictures",
      "directorName": "Christopher Nolan",
      "genres": [],
      "stars": []
    }
  ]
}
```

## 1.3 — Create

The API at url **`/api/director/create`**, method **POST**, inserts a new director:
- Request body:
  ```json
  {
    "fullName": "Dummy Director",
    "male": true,
    "dob": "1990-10-22",
    "nationality": "USA",
    "description": "something"
  }
  ```
- Success: returns the number of records added (body `1`, status **200**).
- Error: any exception during creation → status **409 Conflict**, body (plain text, no quotes): `There is an error while adding.`

## 1.4 — Update

The API at url **`/api/director/update/{id}`**, method **PUT**, updates one director:
- Request body:
  ```json
  {
    "fullName": "Updated Director",
    "male": true,
    "dob": "1990-10-22",
    "nationality": "USA",
    "description": "Updated description"
  }
  ```
- If the director does not exist: status **404**, body (plain text): `Director not found.`
- Success: returns the number of affected records (body `1`, status **200**).
- Any other exception: status **409 Conflict**, body (plain text): `There is an error while updating.`
- The existing entity with `{id}` must be updated; do not insert a new entity.

## 1.5 — Delete

The API at url **`/api/director/delete/{id}`**, method **DELETE**, deletes one director:
- If the director does not exist: status **404**, body (plain text): `Director not found.`
- If the director has one or more movies: status **409 Conflict**, body (plain text): `Cannot delete a director having movies.`
- Success: returns the number of affected records (body `1`, status **200**).
- Any other exception: status **409 Conflict**, body (plain text): `There is an error while deleting.`

## 1.6 — Search by multiple criteria

The API at url below searches directors by multiple optional criteria:

```text
GET /api/director/search?name={name}&nationality={nationality}&gender={gender}&fromDob={fromDob}&toDob={toDob}
```

- Every query parameter is optional.
- `name` performs a case-insensitive contains search on `FullName`.
- `nationality` uses case-insensitive equality.
- `gender` accepts `male`, `female`, or empty. Any other value returns status **400**, body (plain text): `Invalid gender.`
- `fromDob` and `toDob` use `yyyy-MM-dd` and are inclusive.
- All supplied criteria are combined using AND.
- Supplying no criteria returns all directors.
- Sort the result by `FullName` ascending.
- Each result contains `id`, `fullName`, `gender`, `dob`, `dobString`, `nationality`, and `description`.
- No matches returns an empty array with status **200**.
