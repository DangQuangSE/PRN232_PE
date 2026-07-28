# Question 2 — MVC / Razor Pages — Search Products by Multiple Criteria

Build a Razor Pages application at **`/Products/Designer_Product`**. Use `HttpClient` and:

```json
{ "GivenAPIBaseUrl": "http://localhost:5102" }
```

## 1. Given APIs

| Method | API |
|---|---|
| GET | `api/Designers/GetDesigners` |
| GET | `api/Products/GetProducts` |
| GET | `api/Products/GetProductsByDesignerId/{designerId}` |
| GET | `api/Products/SearchProducts?name=&material=&designerId=&fromYear=&toYear=` |

## 2. Display products

Use the same designer list and product table as Set D. Designer links use `di_{designerId}`. Product cells use `td_{columnName}_{productId}`.

## 3.3. Search by multiple criteria

| Criterion | Element | ID | Query parameter |
|---|---|---|---|
| Product name contains | text input | `input_name` | `name` |
| Material contains | text input | `input_material` | `material` |
| Designer | select | `select_designer` | `designerId` |
| Inclusive start year | number input | `input_fromYear` | `fromYear` |
| Inclusive end year | number input | `input_toYear` | `toYear` |
| Search | button | `btn_search` | — |
| Reset | anchor | `btn_reset` | — |

- Call `GET api/Products/SearchProducts` with only the supplied parameters.
- All supplied criteria are combined with AND.
- Name and material are case-insensitive substring searches.
- An empty designer means all designers.
- No criteria displays all products.
- Preserve every entered criterion after searching.
- If `fromYear > toYear`, do not call GivenAPI. Show `From year must not exceed to year.` in element id `search_error`.
- No matches shows `No products found.` in element id `search_message`.

All input and output elements must have an `id`.
