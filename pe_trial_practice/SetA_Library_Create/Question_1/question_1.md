# Question 1 — Web API (Authors)

> Instructions chung: xem [../../README.md](../../README.md)

Chạy script [`database.sql`](../../../given_pe_trial_practice/SetA_Library_Create/1/database.sql) (thư mục given) trên SQL Server trước khi làm bài (tạo database `PE_Practice_LibraryA` kèm dữ liệu mẫu).

**Bảng dữ liệu:**

| Table | Columns (Data Type, Allow Null) |
|---|---|
| **Authors** | Id (int, PK) · FullName (varchar(40)) · Male (bit) · Dob (date) · Nationality (varchar(30)) · Description (ntext) |
| **Books** | Id (int, PK) · Title (varchar(200)) · PublishDate (date, null) · Description (text, null) · Language (varchar(30)) · PublisherId (int, null) · AuthorId (int, null) |
| **Publishers** | Id (int, PK) · Name (varchar(100)) |
| **Translators** | Id (int, PK) · FullName (varchar(100)) · Male (bit, null) · Dob (date, null) · Description (text, null) · Nationality (varchar(30), null) |
| **Genres** | Id (int, PK) · Title (nchar(10)) |
| **Book_Genre** | BookId (int, PK/FK) · GenreId (int, PK/FK) |
| **Book_Translator** | BookId (int, PK/FK) · TranslatorId (int, PK/FK) |

**Quan hệ:** Books n—1 Authors; Books n—1 Publishers; Books 1—n Book_Genre n—1 Genres; Books 1—n Book_Translator n—1 Translators.

**Note that you must:**
- using the root path `http://localhost:5000`
- using database connection string in `appsettings.json`:
  ```json
  { "ConnectionStrings": { "MyCnn": "" } }
  ```

## 1.1

The API at url **`/api/author/getauthors/{nationality}/{gender}`**:
- Receives two strings `{nationality}` and `{gender}`, using **GET**.
- Returns all authors of given nationality and gender.
- `dobString` returned with format **M/d/yyyy**.

Ví dụ (`GET /api/author/getauthors/japan/male`):
```json
[
  {
    "id": 3,
    "fullName": "Haruki Murakami",
    "gender": "Male",
    "dob": "1949-01-12T00:00:00",
    "dobString": "1/12/1949",
    "nationality": "Japan",
    "description": "Japanese writer known for surreal, melancholic novels."
  }
]
```

## 1.2

The API at url **`/api/author/getauthor/{id}`**:
- Receives an integer `{id}`, using **GET**.
- Returns full author info including a list of books they wrote.
- `dobString` format **M/d/yyyy**.
- `genres` and `translators` return **empty list** for every book (do not query the junction tables for this endpoint).
- Returns **404** if the author does not exist.

Ví dụ (`GET /api/author/getauthor/3`):
```json
{
  "id": 3,
  "fullName": "Haruki Murakami",
  "gender": "Male",
  "dob": "1949-01-12T00:00:00",
  "dobString": "1/12/1949",
  "nationality": "Japan",
  "description": "Japanese writer known for surreal, melancholic novels.",
  "books": [
    {
      "id": 3,
      "title": "Norwegian Wood",
      "publishDate": "1987-09-04T00:00:00",
      "publishYear": 1987,
      "description": "A nostalgic story of loss and burgeoning sexuality.",
      "language": "Japanese",
      "publisherId": 3,
      "authorId": 3,
      "publisherName": "Kodansha",
      "authorName": "Haruki Murakami",
      "genres": [],
      "translators": []
    },
    {
      "id": 6,
      "title": "Kafka on the Shore",
      "publishDate": "2002-09-12T00:00:00",
      "publishYear": 2002,
      "description": "A teenage runaway and an aging simpleton's intertwined journeys.",
      "language": "Japanese",
      "publisherId": 3,
      "authorId": 3,
      "publisherName": "Kodansha",
      "authorName": "Haruki Murakami",
      "genres": [],
      "translators": []
    }
  ]
}
```

## 1.3

The API at url **`/api/author/create`**, method **POST**, inserts a new author:
- Request body:
  ```json
  {
    "fullName": "Dummy Author",
    "male": true,
    "dob": "1990-10-22",
    "nationality": "USA",
    "description": "something"
  }
  ```
- Success: returns the number of records added (body `1`, status **200**).
- Error: any exception during creation → status **409 Conflict**, body (plain text, no quotes): `There is an error while adding.`
