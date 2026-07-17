# Question 1 — Web API (Teachers)

> Instructions chung: xem [../../README.md](../../README.md)

Chạy script [`database.sql`](../../../given_pe_trial_practice/SetC_School_Combined/1/database.sql) (thư mục given) trên SQL Server trước khi làm bài (tạo database `PE_Practice_SchoolC` kèm dữ liệu mẫu).

**Bảng dữ liệu:**

| Table | Columns (Data Type, Allow Null) |
|---|---|
| **Teachers** | Id (int, PK) · FullName (varchar(40)) · Male (bit) · Dob (date) · Nationality (varchar(30)) · Description (ntext) |
| **Courses** | Id (int, PK) · Title (varchar(200)) · StartDate (date, null) · Description (text, null) · Language (varchar(30)) · DepartmentId (int, null) · TeacherId (int, null) |
| **Departments** | Id (int, PK) · Name (varchar(100)) |
| **Assistants** | Id (int, PK) · FullName (varchar(100)) · Male (bit, null) · Dob (date, null) · Description (text, null) · Nationality (varchar(30), null) |
| **Tags** | Id (int, PK) · Title (nchar(10)) |
| **Course_Tag** | CourseId (int, PK/FK) · TagId (int, PK/FK) |
| **Course_Assistant** | CourseId (int, PK/FK) · AssistantId (int, PK/FK) |

**Quan hệ:** Courses n—1 Teachers; Courses n—1 Departments; Courses 1—n Course_Tag n—1 Tags; Courses 1—n Course_Assistant n—1 Assistants.

**Note that you must:**
- using the root path `http://localhost:5000`
- using database connection string in `appsettings.json`:
  ```json
  { "ConnectionStrings": { "MyCnn": "" } }
  ```

## 1.1

The API at url **`/api/teacher/getteachers/{nationality}/{gender}`**:
- Receives two strings `{nationality}` and `{gender}`, using **GET**.
- Returns all teachers of given nationality and gender.
- `dobString` returned with format **M/d/yyyy**.

Ví dụ (`GET /api/teacher/getteachers/england/male`):
```json
[
  {
    "id": 3,
    "fullName": "Alan Turing",
    "gender": "Male",
    "dob": "1912-06-23T00:00:00",
    "dobString": "6/23/1912",
    "nationality": "England",
    "description": "Mathematician and computer scientist, father of theoretical computer science."
  }
]
```

## 1.2

The API at url **`/api/teacher/getteacher/{id}`**:
- Receives an integer `{id}`, using **GET**.
- Returns full teacher info including a list of courses they teach.
- `dobString` format **M/d/yyyy**.
- `tags` and `assistants` return **empty list** for every course.
- Returns **404** if the teacher does not exist.

Ví dụ (`GET /api/teacher/getteacher/3`):
```json
{
  "id": 3,
  "fullName": "Alan Turing",
  "gender": "Male",
  "dob": "1912-06-23T00:00:00",
  "dobString": "6/23/1912",
  "nationality": "England",
  "description": "Mathematician and computer scientist, father of theoretical computer science.",
  "courses": [
    {
      "id": 3,
      "title": "Introduction to Computing",
      "startDate": "2024-09-10T00:00:00",
      "startYear": 2024,
      "description": "History and theory of computation.",
      "language": "English",
      "departmentId": 3,
      "teacherId": 3,
      "departmentName": "Computer Science",
      "teacherName": "Alan Turing",
      "tags": [],
      "assistants": []
    },
    {
      "id": 4,
      "title": "Algorithms and Machines",
      "startDate": "2025-01-15T00:00:00",
      "startYear": 2025,
      "description": "Turing machines and algorithmic thinking.",
      "language": "English",
      "departmentId": 3,
      "teacherId": 3,
      "departmentName": "Computer Science",
      "teacherName": "Alan Turing",
      "tags": [],
      "assistants": []
    }
  ]
}
```

## 1.3

The API at url **`/api/teacher/create`**, method **POST**, inserts a new teacher:
- Request body:
  ```json
  {
    "fullName": "Dummy Teacher",
    "male": true,
    "dob": "1990-10-22",
    "nationality": "USA",
    "description": "something"
  }
  ```
- Success: returns the number of records added (body `1`, status **200**).
- Error: any exception during creation → status **409 Conflict**, body (plain text, no quotes): `There is an error while adding.`
