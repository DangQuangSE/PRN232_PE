# Question 2 — MVC / Razor Pages — Search Product by Name

Build a Razor Pages application at **`/Products/Designer_Product`**. Data must be obtained with `HttpClient`. Put this setting in `appsettings.json`:

```json
{ "GivenAPIBaseUrl": "http://localhost:5101" }
```

## 1. Given APIs

| Method | API |
|---|---|
| GET | `api/Designers/GetDesigners` |
| GET | `api/Products/GetProducts` |
| GET | `api/Products/GetProductsByDesignerId/{designerId}` |
| GET | `api/Products/SearchProducts?name={name}` |

## 2. Display products

- Display all designers on the left. Each filter link has id `di_{designerId}`.
- Display products in a table: Name, Launch Date (`MM/dd/yyyy`), Description, Material, Designer, Reviewers.
- Every cell has id `td_{columnName}_{productId}` where columnName is `name`, `launchDate`, `description`, `material`, `designer`, or `reviewers`.
- Reviewers are comma-separated without spaces.

## 3.3. Search by one criterion

- Add a GET form targeting `/Products/Designer_Product`.
- Text input id `input_name`, query parameter `name`.
- Button id `btn_search`, inner text `Search`.
- Anchor id `btn_reset`, inner text `Reset`, linking to `/Products/Designer_Product`.
- Submit calls `GET api/Products/SearchProducts?name={name}`.
- Search is case-insensitive and matches a substring.
- Empty input displays all products.
- Preserve the entered name after searching.
- If no products match, keep `<tbody id="product_rows">` empty and show `No products found.` in element id `search_message`.

All input and output elements must have an `id`.
