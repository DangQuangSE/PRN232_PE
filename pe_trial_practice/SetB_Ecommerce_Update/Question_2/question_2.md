# Question 2 — MVC / Razor Pages (Products by Designer) — Update

> Instructions chung: xem [../../README.md](../../README.md)
> Phần đầu câu này nằm chung trang với cuối Question 1.

In this question, you are asked to write MVC/Razor Pages model that shows information about products, and allows **editing an existing product**. Data is fetched/written by calling pre-existing RESTful APIs hosted at **GivenAPIBaseUrl** (see [../../README.md](../../README.md) for the note about this practice set not shipping a runnable GivenAPI).

## 1. Given APIs include

| API | Method | Note |
|---|---|---|
| `api/Designers/GetDesigners` | GET | Return all designers |
| `api/Products/GetProducts` | GET | Return all products, each: `id, name, launchDate, description, material, designer: {id, fullName}, reviewers: [{id, fullName}], tags: [{id, title}]` |
| `api/Products/GetProductsByDesignerId/{designerId}` | GET | Return products of one designer |
| `api/Products/GetProductById/{id}` | GET | Return one product (used to pre-fill the edit form) |
| `api/Products/UpdateProduct/{id}` | PUT | Body: `{ "name": "...", "launchDate": "yyyy-MM-dd", "description": "...", "material": "...", "designerId": 0 }`. Returns the updated product (status 200) or 404 if not found. |

## 2. Note (IMPORTANT)

- Students **MUST** use `HttpClient` to call the API.
- `GivenAPIBaseUrl` must be in `appsettings.json`:
  ```json
  { "GivenAPIBaseUrl": "http://localhost:5100" }
  ```
- All input and output elements in the HTML source must have an **'id'** attribute.

## 3. Requirements

The web application has a page at url **`/Products/Designer_Product`**, which includes three main parts.

### 3.1. Display List of Products

On first access, display all products in table format:

- Name, Launch Date (`MM/dd/yyyy`), Description, Material, Designer (from Designers), Reviewers (comma-separated, no spaces), Action.
- Each `<td>` has id **`td_{columnName}_{productId}`** (camelCase columnName: `name`, `launchDate`, `description`, `material`, `designer`, `reviewers`).
- Each row's Action cell has an `<a>` **Edit** link with id **`btn_edit_{productId}`**.

### 3.2. Display all Designers and Filter by Designer

List of Designers on the **left**, each `<a>` with id **`di_{designerId}`**. Clicking a designer link filters the table to only that designer's products (calls `GetProductsByDesignerId`).

### 3.3. Update an Existing Product (main operation of this question)

Clicking a row's `btn_edit_{productId}` link navigates to **`/Products/Designer_Product?editId={productId}`**. When `editId` is present:

- Call `GetProductById(editId)` and render an edit form **pre-filled** with the current values, above or instead of the table:

| Field | Element | Id |
|---|---|---|
| Hidden product id | `<input type="hidden">` | `input_id` |
| Name | `<input type="text">`, value = current name | `input_name` |
| Launch Date | `<input type="date">`, value = current launch date | `input_launchDate` |
| Description | `<textarea>`, content = current description | `input_description` |
| Material | `<input type="text">`, value = current material | `input_material` |
| Designer | `<select>` (options = all designers), selected = current designer | `select_designer` |
| Save | `<button>`, inner text `Save` | `btn_save` |

- On submit, call `PUT api/Products/UpdateProduct/{id}` with the form's current values.
- On success, redirect to `/Products/Designer_Product` (no query string) — the table reloads and the edited row's `td_*_{productId}` cells reflect the new values.
- On failure (GivenAPI returns 404 or any error), redisplay the edit form with the values the user entered still filled in, and show an error message in an element with id `edit_error`.

## 4. HTML Elements ID — Summary

| Element | Element Tag | Id |
|---|---|---|
| Each cell in the table | `<td>` | `td_{columnName}_{productId}` |
| Edit link | `<a>` | `btn_edit_{productId}` |
| Link to filter Products of Designer | `<a>` | `di_{designerId}` |
| Hidden id input (edit form) | `<input>` | `input_id` |
| Name input | `<input>` | `input_name` |
| Launch Date input | `<input>` | `input_launchDate` |
| Description input | `<textarea>` | `input_description` |
| Material input | `<input>` | `input_material` |
| Designer select | `<select>` | `select_designer` |
| Save button | `<button>` | `btn_save` |
| Error message container | any | `edit_error` |
