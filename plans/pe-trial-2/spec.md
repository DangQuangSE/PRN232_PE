# Spec: PE Trial (Paper No. 2) — Question 1 Web API + Question 2 Razor Pages

**Date:** 2026-07-16
**Status:** Ready

---

## Problem Statement

Đề thi thực hành (PE) yêu cầu xây 2 ứng dụng độc lập dựa trên schema phim/đạo diễn có sẵn: (1) một Web API quản lý Director/Movie chạy trên SQL Server, và (2) một trang Razor Pages hiển thị/lọc/xóa phim bằng cách gọi một API có sẵn (`givenAPI`, không được sửa). Cả hai phải khớp chính xác định dạng response, HTML id, và mã lỗi được đề bài quy định để đạt điểm tối đa.

---

## User Stories

- **[P1]** Là giám khảo chấm bài, tôi muốn gọi `GET /api/director/getdirectors/{nationality}/{gender}` để lấy danh sách director lọc theo quốc tịch + giới tính.
  Accepted when: `GET /api/director/getdirectors/usa/male` trả về mảng JSON đúng field `id, fullName, gender, dob, dobString (M/d/yyyy), nationality, description`, chỉ gồm director USA + Male.

- **[P1]** Là giám khảo, tôi muốn gọi `GET /api/director/getdirector/{id}` để lấy chi tiết 1 director kèm danh sách phim đã đạo diễn.
  Accepted when: response gồm đủ field director + mảng `movies` (mỗi movie có `genres: []`, `stars: []`); id không tồn tại → 404.

- **[P1]** Là giám khảo, tôi muốn gọi `POST /api/director/create` để thêm director mới.
  Accepted when: thành công trả về số bản ghi đã thêm (thường là `1`, status 200); khi có exception khi insert → status 409 Conflict, body `"There is an error while adding."`.

- **[P1]** Là người dùng cuối, tôi muốn vào `/Movies/Director_Movie` và thấy toàn bộ danh sách phim ngay lần đầu truy cập.
  Accepted when: bảng hiển thị Title, Release Date (`MM/dd/yyyy`), Description, Language, Director, Stars (nối bằng `,` không khoảng trắng), nút Delete; mỗi `<td>` có id `td_{columnName}_{movieId}` (camelCase), nút xóa có id `btn_delete_{movieId}` với text "Delete".

- **[P1]** Là người dùng cuối, tôi muốn click vào 1 director ở panel trái để lọc danh sách phim chỉ của director đó.
  Accepted when: mỗi director hiển thị dạng `<a id="di_{directorId}">`; click vào link → bảng phim chỉ còn phim của director đó (dùng API `GetMoviesByDirectorId`).

- **[P1]** Là người dùng cuối, tôi muốn xóa 1 phim và thấy danh sách cập nhật ngay.
  Accepted when: click Delete → gọi `DELETE /api/Movies/DeleteMovie/{id}` trên givenAPI → redirect về `/Movies/Director_Movie` → phim đã xóa không còn xuất hiện.

- **[P3]** _(out of scope)_ Authentication/authorization cho cả 2 API — đề không yêu cầu.

---

## Functional Requirements

### Question 1 — Web API (`solution/Q1_WebAPI`)

1. FR-01: Project ASP.NET Core Web API (.NET 8), chạy tại `http://localhost:5000` (cấu hình trong `launchSettings.json`).
2. FR-02: EF Core DbContext map đúng 7 bảng từ `given_pe_trial/1/database.sql` (Directors, Movies, Producers, Stars, Genres, Movie_Genre — composite key, Movie_Star — composite key). Connection string đọc từ `appsettings.json` → `ConnectionStrings:MyCnn`.
3. FR-03: `GET /api/director/getdirectors/{nationality}/{gender}` — lọc case-insensitive theo `Nationality` và `Male` (gender string "male"/"female" → bool); trả `dobString` format `M/d/yyyy` (dùng `dob.ToString("M/d/yyyy")`).
4. FR-04: `GET /api/director/getdirector/{id}` — trả director + `movies` (từ `Movies.DirectorId == id`), mỗi movie có `genres: []`, `stars: []` cố định (không query thật Movie_Genre/Movie_Star cho endpoint này). Trả 404 nếu director không tồn tại.
5. FR-05: `POST /api/director/create` — nhận body `{fullName, male, dob, nationality, description}`, insert vào DB, trả về số record đã thêm (int). Bọc try/catch quanh `SaveChanges()`; bắt exception → trả `409 Conflict` với body string `"There is an error while adding."`.
6. FR-06: Không thêm NuGet package ngoài những gì cần thiết tối thiểu cho EF Core SQL Server (Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Design).

### Question 2 — Razor Pages (`solution/Q2_MovieApp`)

7. FR-07: Project ASP.NET Core Razor Pages (.NET 8) độc lập, gọi sang `givenAPI` (project có sẵn tại `given_pe_trial/2/givenAPI`, chạy port 5100, **không sửa**) qua `HttpClient` (dùng `IHttpClientFactory`, base URL đọc từ `appsettings.json` → `GivenAPIBaseUrl`).
8. FR-08: Page `Pages/Movies/Director_Movie.cshtml` — route `/Movies/Director_Movie`.
9. FR-09: Lần đầu load (không query string) → gọi `GET /api/Movies/GetMovies` (đã có `MovieResponse` gồm director/stars/genres) và `GET /api/Directors/GetDirectors` để build panel trái.
10. FR-10: Khi có `?directorId={id}` → gọi `GET /api/Movies/GetMoviesByDirectorId/{id}` thay vì GetMovies.
11. FR-11: Mỗi `<td>` render với id `td_{columnName}_{movieId}` — columnName camelCase: `title, releaseDate, description, language, director, stars`. Release Date format `MM/dd/yyyy`. Stars nối bằng `,` không khoảng trắng (`string.Join(",", starNames)`).
12. FR-12: Nút Delete `<a id="btn_delete_{movieId}">Delete</a>` — trỏ tới handler Razor Page (`OnPostDeleteAsync` hoặc GET handler `?handler=Delete&id=`) gọi `DELETE /api/Movies/DeleteMovie/{id}` trên givenAPI rồi redirect về `/Movies/Director_Movie` (giữ nguyên filter director nếu đang lọc — theo đề chỉ yêu cầu quay về trang gốc, nên redirect không kèm query string là chấp nhận được).
13. FR-13: Panel trái: mỗi director là `<a id="di_{directorId}">{fullName}</a>`, click → điều hướng `/Movies/Director_Movie?directorId={id}`.

---

## Non-Functional Requirements

- Performance: không yêu cầu cụ thể (dữ liệu nhỏ, <10 phim).
- Security: không yêu cầu auth; CORS đã mở sẵn ở givenAPI, Q1 API không cần CORS đặc biệt trừ khi Q2 gọi cross-origin nội bộ server-side (HttpClient server-to-server nên không bị CORS chặn).
- Availability: chạy local only (không cần cấu hình deploy/production).

---

## Success Criteria

- [ ] Q1: `GET /api/director/getdirectors/usa/male` trả đúng danh sách director USA+Male với `dobString` đúng format `M/d/yyyy` (verify bằng ví dụ David Gordon Green → `4/9/1975`).
- [ ] Q1: `GET /api/director/getdirector/5` trả Mike Barker kèm 2 movies (Luckiest Girl Alive, Broadchurch), `genres`/`stars` rỗng.
- [ ] Q1: `POST /api/director/create` với body hợp lệ trả về `1`; khi DB lỗi (vd stop SQL Server) trả 409 + đúng message.
- [ ] Q2: Truy cập `/Movies/Director_Movie` lần đầu hiển thị đủ 7 movie từ givenAPI, đúng toàn bộ id theo convention.
- [ ] Q2: Click `di_2` (Aaron Horvath) → bảng chỉ còn "The Super Mario Bros. Movie".
- [ ] Q2: Click `btn_delete_{id}` → phim biến mất khỏi danh sách sau khi quay lại trang.

---

## Out of Scope

- Authentication/authorization.
- Sửa đổi code trong `given_pe_trial/2/givenAPI` (đề cấm).
- Thêm NuGet package ngoài mức tối thiểu cần thiết.
- Q2 dùng AJAX/JS filter — chọn full page reload qua query string để giảm rủi ro lỗi JS trong phòng thi (có thể nâng cấp AJAX sau nếu còn thời gian, nhưng không phải yêu cầu bắt buộc).

---

## Assumptions

- `given_pe_trial/1/database.sql` là script SQL server chính thức cho Question 1 (đã verify khớp 100% với schema mô tả trong `question_1.md`).
- Ví dụ response 1.2 trong đề (`question_1.md`) có movie thứ 2 thiếu vài field — coi là ví dụ bị rút gọn trong tài liệu, thực tế mọi movie trả về đủ field như movie thứ nhất.
- SQL Server đã cài sẵn (LocalDB hoặc SQL Server instance) trên máy chạy — không thuộc phạm vi spec này để cấu hình cài đặt SQL Server.
- Project đặt tại `solution/Q1_WebAPI` và `solution/Q2_MovieApp` ở root repo (theo lựa chọn user), tách biệt khỏi `given_pe_trial/` và `pe_trial/` (giữ nguyên để tham chiếu đề + script gốc).
