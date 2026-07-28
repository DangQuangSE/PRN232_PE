# Question 1 — Web API (Designers)

> Instructions chung: xem [../../README.md](../../README.md)

Chạy script [`database.sql`](../../../given_pe_trial_practice/SetF_Ecommerce_Delete/1/database.sql) (thư mục given) trên SQL Server trước khi làm bài (tạo database `PE_Practice_EcommerceF` kèm dữ liệu mẫu).

**Bảng dữ liệu:**

| Table | Columns (Data Type, Allow Null) |
|---|---|
| **Designers** | Id (int, PK) · FullName (varchar(40)) · Male (bit) · Dob (date) · Nationality (varchar(30)) · Description (ntext) |
| **Products** | Id (int, PK) · Name (varchar(200)) · LaunchDate (date, null) · Description (text, null) · Material (varchar(30)) · ManufacturerId (int, null) · DesignerId (int, null) |
| **Manufacturers** | Id (int, PK) · Name (varchar(100)) |
| **Reviewers** | Id (int, PK) · FullName (varchar(100)) · Male (bit, null) · Dob (date, null) · Description (text, null) · Nationality (varchar(30), null) |
| **Tags** | Id (int, PK) · Title (nchar(10)) |
| **Product_Tag** | ProductId (int, PK/FK) · TagId (int, PK/FK) |
| **Product_Reviewer** | ProductId (int, PK/FK) · ReviewerId (int, PK/FK) |

**Quan hệ:** Products n—1 Designers; Products n—1 Manufacturers; Products 1—n Product_Tag n—1 Tags; Products 1—n Product_Reviewer n—1 Reviewers.

**Note that you must:**
- using the root path `http://localhost:5000`
- using database connection string in `appsettings.json`:
  ```json
  { "ConnectionStrings": { "MyCnn": "" } }
  ```

## 1.1

The API at url **`/api/designer/getdesigners/{nationality}/{gender}`**:
- Receives two strings `{nationality}` and `{gender}`, using **GET**.
- Returns all designers of given nationality and gender.
- `dobString` returned with format **M/d/yyyy**.

Ví dụ (`GET /api/designer/getdesigners/england/male`):
```json
[
  {
    "id": 1,
    "fullName": "Jony Ive",
    "gender": "Male",
    "dob": "1967-02-27T00:00:00",
    "dobString": "2/27/1967",
    "nationality": "England",
    "description": "Former Chief Design Officer at Apple, known for the iPhone and iMac."
  }
]
```

## 1.2

The API at url **`/api/designer/getdesigner/{id}`**:
- Receives an integer `{id}`, using **GET**.
- Returns full designer info including a list of products they designed.
- `dobString` format **M/d/yyyy**.
- `tags` and `reviewers` return **empty list** for every product.
- Returns **404** if the designer does not exist.

Ví dụ (`GET /api/designer/getdesigner/1`):
```json
{
  "id": 1,
  "fullName": "Jony Ive",
  "gender": "Male",
  "dob": "1967-02-27T00:00:00",
  "dobString": "2/27/1967",
  "nationality": "England",
  "description": "Former Chief Design Officer at Apple, known for the iPhone and iMac.",
  "products": [
    {
      "id": 1,
      "name": "iPhone 12",
      "launchDate": "2020-10-23T00:00:00",
      "launchYear": 2020,
      "description": "Smartphone with A14 Bionic chip.",
      "material": "Aluminum",
      "manufacturerId": 1,
      "designerId": 1,
      "manufacturerName": "Apple Inc.",
      "designerName": "Jony Ive",
      "tags": [],
      "reviewers": []
    },
    {
      "id": 2,
      "name": "MacBook Air",
      "launchDate": "2008-01-15T00:00:00",
      "launchYear": 2008,
      "description": "Ultra-thin laptop.",
      "material": "Aluminum",
      "manufacturerId": 1,
      "designerId": 1,
      "manufacturerName": "Apple Inc.",
      "designerName": "Jony Ive",
      "tags": [],
      "reviewers": []
    }
  ]
}
```

## 1.3

The API at url below searches designers by multiple optional criteria:

```text
GET /api/designer/search?name={name}&nationality={nationality}&gender={gender}&fromDob={fromDob}&toDob={toDob}
```

- Every query parameter is optional.
- `name` performs a case-insensitive contains search on `FullName`.
- `nationality` uses case-insensitive equality.
- `gender` accepts `male`, `female`, or empty. Any other value returns status **400**, body (plain text): `Invalid gender.`
- `fromDob` and `toDob` use `yyyy-MM-dd` and are inclusive.
- All supplied criteria are combined using AND.
- Supplying no criteria returns all designers.
- Sort the result by `FullName` ascending.
- Each result contains `id`, `fullName`, `gender`, `dob`, `dobString`, `nationality`, and `description`.
- No matches returns an empty array with status **200**.

