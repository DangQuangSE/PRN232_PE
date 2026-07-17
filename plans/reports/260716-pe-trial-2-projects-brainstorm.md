# Brainstorm: Hoàn thành PE Trial (Paper No. 2) — Question 1 (Web API) + Question 2 (Razor Pages)

**Date:** 2026-07-16

## Ideas Explored

- **Q1 DB approach:** Code-First (tự viết Entity + Migration) vs Database-First (chạy script `database.sql` có sẵn rồi map Entity theo đúng schema). → Chọn dùng script có sẵn (`given_pe_trial/1/database.sql`), viết EF Core model tay khớp đúng cột/kiểu dữ liệu, không cần scaffold tool vì schema đơn giản (7 bảng).
- **Q2 kiến trúc:** MVC Controller+View vs Razor Pages. → Chọn Razor Pages (`Pages/Movies/Director_Movie.cshtml`) theo yêu cầu route `/Movies/Director_Movie`.
- **Q2 filter/delete UX:** Full page reload (query string `?directorId=`) vs AJAX/fetch. Đề không bắt buộc AJAX — chỉ yêu cầu đúng id và hành vi cuối cùng (filter đúng, xóa xong quay về trang gốc và cập nhật). Full page reload qua `<a href>`/query string là đơn giản nhất và chắc chắn khớp yêu cầu, tránh rủi ro JS lỗi trong phòng thi.
- **Vị trí project mới:** đặt lồng trong `given_pe_trial/1` và `given_pe_trial/2` (cạnh script/givenAPI) vs tạo thư mục riêng ở root. → User chọn tạo thư mục riêng ở root (`solution/`), tách khỏi cấu trúc đề bài gốc để dễ quản lý.

## User's Direction

- DB: dùng script SQL có sẵn trong đề (`given_pe_trial/1/database.sql`) — Database-First, không tự sinh migration.
- Q2: dùng Razor Pages, không dùng MVC Controller+View.
- Vị trí: tạo 2 project mới trong thư mục riêng ở root repo (`solution/`), không đụng vào `given_pe_trial/` (giữ nguyên givenAPI + script gốc để tham chiếu).

## Open Questions

- Tên chính xác của 2 project mới (đề xuất: `solution/Q1_WebAPI` và `solution/Q2_MovieApp`) — cần chốt khi lập plan.
- Đề yêu cầu "must use the given solution" ở README chung — chỉ áp dụng cho givenAPI (Q2, không được sửa); Q1 không có given solution nên tự tạo mới từ đầu là hợp lệ.
- `dobString` format `M/d/yyyy` (không có số 0 đứng trước) — cần dùng `dob.ToString("M/d/yyyy")` chuẩn .NET, không phải `MM/dd/yyyy`.
- Ví dụ response 1.2 có movie thứ 2 thiếu field `producerName/directorName/genres/stars` — coi đây là ví dụ bị cắt ngắn trong tài liệu, không phải khác schema thật; mọi movie trả về đều đủ field với `genres: []`, `stars: []`.

## Risks

- **Format ngày `dobString`:** dễ nhầm giữa `M/d/yyyy` (đề yêu cầu) và `MM/dd/yyyy` (Q2 dùng format này cho Release Date) — hai format khác nhau ở hai câu hỏi, phải tách rõ helper riêng.
- **Lỗi 409 Conflict:** phải bắt exception cụ thể quanh thao tác insert (vd `DbUpdateException`) và trả đúng message `"There is an error while adding."`, không phải để lỗi 500 mặc định.
- **HTML id conventions (Q2):** id phải chính xác tuyệt đối (`td_{camelCase}_{movieId}`, `btn_delete_{movieId}`, `di_{directorId}`) vì đây là tiêu chí chấm điểm tự động — sai id là mất điểm dù logic đúng.
