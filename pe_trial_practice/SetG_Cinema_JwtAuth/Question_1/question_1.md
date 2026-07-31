# Question 1 — Web API (Directors) + JWT Authentication

> Instructions chung: xem [../../README.md](../../README.md)

Chạy script [`database.sql`](../../../given_pe_trial_practice/SetG_Cinema_JwtAuth/1/database.sql) (thư mục given) trên SQL Server trước khi làm bài (tạo database `PE_Practice_CinemaG` kèm dữ liệu mẫu).

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
| **Accounts** | Id (int, PK) · Username (varchar(50), unique) · Password (varchar(100)) · Role (varchar(20)) |

**Quan hệ:** Movies n—1 Directors; Movies n—1 Producers; Movies 1—n Movie_Genre n—1 Genres; Movies 1—n Movie_Star n—1 Stars. `Accounts` là bảng độc lập, chỉ dùng để đăng nhập lấy JWT.

**Note that you must:**
- using the root path `http://localhost:5000`
- using database connection string in `appsettings.json`:
  ```json
  { "ConnectionStrings": { "MyCnn": "" } }
  ```
- using JWT config in `appsettings.json` (giá trị bắt buộc, copy nguyên văn):
  ```json
  {
    "Jwt": {
      "Issuer": "http://fpt.edu.vn",
      "Audience": "http://localhost:5000",
      "Key": "Practical Exam - PRN231 - Summer 2024 - Computing Fundamental Department - FPT University",
      "ExpiryInDays": 1
    }
  }
  ```
- Sinh viên **không được** lấy URL của API theo bất cứ cách nào khác ngoài các endpoint dưới đây.

## 1.1

The API at url **`/api/director/getdirectors/{nationality}/{gender}`**:
- Receives two strings `{nationality}` and `{gender}`, using **GET**.
- Returns all directors of given nationality and gender.
- `dobString` returned with format **M/d/yyyy**.
- **Public** — no JWT required.

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
- **Public** — no JWT required.

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
    },
    {
      "id": 2,
      "title": "The Dark Knight",
      "releaseDate": "2008-07-18T00:00:00",
      "releaseYear": 2008,
      "description": "Batman faces the Joker in Gotham City.",
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

## 1.3 — Login (issue JWT)

The API at url **`/api/auth/login`**, method **POST**:
- Request body:
  ```json
  { "username": "admin", "password": "Admin@123" }
  ```
- Checks `Username`/`Password` against table `Accounts` (exact match, case-sensitive).
- Success: status **200**, body:
  ```json
  { "token": "<a valid JWT signed with the Jwt:Key config above, containing at least the username and role claims, expiring after Jwt:ExpiryInDays>" }
  ```
- Failure (username/password không khớp bản ghi nào): status **401 Unauthorized**, body (plain text, no quotes): `Invalid username or password.`

## 1.4 — Create Director (JWT-protected, Admin only)

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
- **Requires a valid JWT** in header `Authorization: Bearer {token}`, obtained from 1.3, **with role `Admin`**.
  - No token / invalid or expired token → status **401 Unauthorized**.
  - Valid token nhưng role khác `Admin` (ví dụ `User`) → status **403 Forbidden**.
- Success (role hợp lệ, thêm thành công): returns the number of records added (body `1`, status **200**).
- Error: any exception during creation → status **409 Conflict**, body (plain text, no quotes): `There is an error while adding.`
