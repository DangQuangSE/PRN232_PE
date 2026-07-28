# Question 2 — MVC / Razor Pages — Delete Product

Build a Razor Pages application at **`/Products/Designer_Product`**. Use `HttpClient` and:

```json
{ "GivenAPIBaseUrl": "http://localhost:5103" }
```

## 1. Given APIs

| Method | API |
|---|---|
| GET | `api/Designers/GetDesigners` |
| GET | `api/Products/GetProducts` |
| GET | `api/Products/GetProductsByDesignerId/{designerId}` |
| DELETE | `api/Products/DeleteProduct/{id}` |

## 2. Display and filter

- Display all designers on the left with link id `di_{designerId}`.
- Clicking a designer filters products through `GetProductsByDesignerId`.
- Display Name, Launch Date (`MM/dd/yyyy`), Description, Material, Designer, Reviewers, and Action.
- Every data cell has id `td_{columnName}_{productId}`.
- Reviewers are comma-separated without spaces.

## 3.3. Delete a product

- Every row has a real `<a>` element with id `btn_delete_{productId}` and inner text `Delete`.
- Clicking it causes Q2 to call `DELETE api/Products/DeleteProduct/{productId}`.
- Success (`204 No Content`): redirect to `/Products/Designer_Product`; the deleted row is absent.
- Failure: redisplay the list and show an error in element id `delete_error`.
- Do not use JavaScript/AJAX. A GET-based Razor Page handler may receive `deleteId`, but the outgoing request to GivenAPI must use HTTP DELETE.

All input and output elements must have an `id`.
