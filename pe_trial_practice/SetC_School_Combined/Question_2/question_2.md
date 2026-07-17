# Question 2 — MVC / Razor Pages (Courses by Teacher) — Create + Update + Delete

> Instructions chung: xem [../../README.md](../../README.md)
> Phần đầu câu này nằm chung trang với cuối Question 1.

In this question, you are asked to write MVC/Razor Pages model that shows information about courses, and supports **creating**, **editing**, and **deleting** courses — all on one page. Data is fetched/written by calling pre-existing RESTful APIs hosted at **GivenAPIBaseUrl** (see [../../README.md](../../README.md) for the note about this practice set not shipping a runnable GivenAPI).

## 1. Given APIs include

| API | Method | Note |
|---|---|---|
| `api/Teachers/GetTeachers` | GET | Return all teachers |
| `api/Courses/GetCourses` | GET | Return all courses, each: `id, title, startDate, description, language, teacher: {id, fullName}, assistants: [{id, fullName}], tags: [{id, title}]` |
| `api/Courses/GetCoursesByTeacherId/{teacherId}` | GET | Return courses of one teacher |
| `api/Courses/GetCourseById/{id}` | GET | Return one course (used to pre-fill the edit form) |
| `api/Courses/CreateCourse` | POST | Body: `{ "title": "...", "startDate": "yyyy-MM-dd", "description": "...", "language": "...", "teacherId": 0 }`. Returns the created course (status 201). |
| `api/Courses/UpdateCourse/{id}` | PUT | Same body shape as create. Returns the updated course (status 200) or 404. |
| `api/Courses/DeleteCourse/{id}` | DELETE | Deletes the course. Returns 204 No Content, or 404 if not found. |

## 2. Note (IMPORTANT)

- Students **MUST** use `HttpClient` to call the API.
- `GivenAPIBaseUrl` must be in `appsettings.json`:
  ```json
  { "GivenAPIBaseUrl": "http://localhost:5100" }
  ```
- All input and output elements in the HTML source must have an **'id'** attribute.
- This is the hardest of the three practice sets: three independent write operations share one page. Keep the id namespaces for the Create form and the Edit form **strictly separate** (see §3.4) so a grader script can locate each one unambiguously.

## 3. Requirements

The web application has a page at url **`/Courses/Teacher_Course`**, which includes four main parts.

### 3.1. Display List of Courses

On first access, display all courses in table format:

- Title, Start Date (`MM/dd/yyyy`), Description, Language, Teacher (from Teachers), Assistants (comma-separated, no spaces), Action.
- Each `<td>` has id **`td_{columnName}_{courseId}`** (camelCase columnName: `title`, `startDate`, `description`, `language`, `teacher`, `assistants`).
- Each row's Action cell has two `<a>` links: **Edit** with id **`btn_edit_{courseId}`**, and **Delete** with id **`btn_delete_{courseId}`** (inner text `Delete`).

### 3.2. Display all Teachers and Filter by Teacher

List of Teachers on the **left**, each `<a>` with id **`di_{teacherId}`**. Clicking a teacher link filters the table to only that teacher's courses (calls `GetCoursesByTeacherId`).

### 3.3. Delete a Course

Clicking `btn_delete_{courseId}` calls `DELETE api/Courses/DeleteCourse/{courseId}` on GivenAPI, then returns to `/Courses/Teacher_Course` with the deleted course no longer in the table (same pattern as the original exam's Question 2 — the Delete control must be a real `<a>` tag, so implement this via a GET-based page handler, not JavaScript/AJAX).

### 3.4. Create and Update a Course

**Create form** (always visible below the table), id namespace prefixed `create_`:

| Field | Element | Id |
|---|---|---|
| Title | `<input type="text">` | `create_input_title` |
| Start Date | `<input type="date">` | `create_input_startDate` |
| Description | `<textarea>` | `create_input_description` |
| Language | `<input type="text">` | `create_input_language` |
| Teacher | `<select>` (options = all teachers) | `create_select_teacher` |
| Submit | `<button>`, inner text `Create` | `btn_create` |

On submit: `POST api/Courses/CreateCourse`, then redirect to `/Courses/Teacher_Course` (table now includes the new course).

**Edit form**, shown only when the page is accessed as `/Courses/Teacher_Course?editId={courseId}` (triggered by `btn_edit_{courseId}`), pre-filled from `GetCourseById(editId)`, id namespace prefixed `edit_`:

| Field | Element | Id |
|---|---|---|
| Hidden course id | `<input type="hidden">` | `edit_input_id` |
| Title | `<input type="text">`, value = current title | `edit_input_title` |
| Start Date | `<input type="date">`, value = current start date | `edit_input_startDate` |
| Description | `<textarea>`, content = current description | `edit_input_description` |
| Language | `<input type="text">`, value = current language | `edit_input_language` |
| Teacher | `<select>`, selected = current teacher | `edit_select_teacher` |
| Save | `<button>`, inner text `Save` | `btn_save` |

On submit: `PUT api/Courses/UpdateCourse/{id}`, then redirect to `/Courses/Teacher_Course` (no query string — table reloads, edited row reflects new values).

On any failure for either form (GivenAPI returns 400/404), redisplay that same form with entered values retained, and show an error in `create_error` or `edit_error` respectively.

## 4. HTML Elements ID — Summary

| Element | Element Tag | Id |
|---|---|---|
| Each cell in the table | `<td>` | `td_{columnName}_{courseId}` |
| Edit link | `<a>` | `btn_edit_{courseId}` |
| Delete link | `<a>` | `btn_delete_{courseId}` |
| Link to filter Courses of Teacher | `<a>` | `di_{teacherId}` |
| Create: title/date/desc/lang/teacher/submit | various | `create_input_title`, `create_input_startDate`, `create_input_description`, `create_input_language`, `create_select_teacher`, `btn_create` |
| Create error container | any | `create_error` |
| Edit: hidden id/title/date/desc/lang/teacher/save | various | `edit_input_id`, `edit_input_title`, `edit_input_startDate`, `edit_input_description`, `edit_input_language`, `edit_select_teacher`, `btn_save` |
| Edit error container | any | `edit_error` |
