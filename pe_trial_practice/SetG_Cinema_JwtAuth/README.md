# PE Trial Practice — Set G: Cinema (Director / Movie) — JWT Auth

> Instructions chung: xem [../README.md](../README.md)

Chủ đề: điện ảnh (Director/Movie/Producer), cùng schema với `solution_pe_practice/PE_PRN232_GivenSolution` — dùng để tập dượt đúng bộ entity mà given chính thức của trường đang dùng. Khác với Set A–F, Question 1 của set này thêm yêu cầu **bảo mật API bằng JWT** (dạng câu hỏi số 2 trong tài liệu hướng dẫn), thay vì chỉ CRUD thuần. Question 2 vẫn là **Create Movie** qua form, gọi GivenAPI riêng (không JWT).

- [Question_1/question_1.md](Question_1/question_1.md) — Web API (ASP.NET Core) + JWT login/protect, có DB schema (script tại `given_pe_trial_practice/SetG_Cinema_JwtAuth/1/database.sql`).
- [Question_2/question_2.md](Question_2/question_2.md) — MVC/Razor Pages, gọi GivenAPI, thao tác chính: **Create Movie**.

**Given materials:** `given_pe_trial_practice/SetG_Cinema_JwtAuth/` — chứa `1/database.sql` (kèm bảng `Accounts` cho JWT login) và `2/givenAPI` (chạy tại `http://localhost:5104`, không yêu cầu JWT — chỉ phục vụ Question 2) + `2/sample.html`.

## Lưu ý riêng cho phần JWT (Question 1)

- Cấu hình `Jwt` trong `appsettings.json` của **Q1** (project bạn tự viết, không phải GivenAPI):
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
- Tài khoản đăng nhập nằm trong bảng `Accounts` (seed sẵn trong `database.sql`): `admin` / `Admin@123` (role `Admin`), `staff` / `Staff@123` (role `User`).
- Sinh viên **không được** lấy URL của API theo bất cứ cách nào khác ngoài các endpoint được liệt kê trong đề.
