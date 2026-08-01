# PE Trial Practice — Set H: Cinema (Director / Movie) — Full Combined Review

> Instructions chung: xem [../README.md](../README.md)

Chủ đề: điện ảnh (Director/Movie/Producer), **cùng domain với đề thi thật gốc** (`../../pe_trial/`) — không phải domain tự chọn. Set này không phải một đề mới, mà là bài **tổng ôn tập**: gộp lại **tất cả các thao tác** đã xuất hiện rải rác ở Set A–F (Create, Update, Delete, Search 1 tiêu chí, Search nhiều tiêu chí) vào **một cặp Q1 + Q2 duy nhất**, để luyện đủ mọi khả năng có thể gặp trong đề thật — trừ JWT (đã có set riêng ở [SetG_Cinema_JwtAuth](../SetG_Cinema_JwtAuth/)).

Ánh xạ thao tác đã gộp — thao tác nào lấy từ set nào:

| Thao tác | Lấy từ | Vị trí trong Set H |
|---|---|---|
| Get list theo 2 tiêu chí cố định (nationality/gender) | Đề gốc + tất cả set A–F | Q1.1 |
| Get by id kèm danh sách con | Đề gốc + tất cả set A–F | Q1.2 |
| Create | Đề gốc, SetA, SetB, SetC | Q1.3 |
| Update | SetE | Q1.4 |
| Delete (có rule chặn xoá khi còn con) | SetD | Q1.5 |
| Search nhiều tiêu chí (do sinh viên tự viết trong Q1) | SetF | Q1.6 |
| Search 1 tiêu chí (phía GivenAPI, sinh viên chỉ gọi) | SetD | Q2 §3.3 |
| Search nhiều tiêu chí (phía GivenAPI, sinh viên chỉ gọi) | SetE | Q2 §3.3 |
| Create qua form | SetA, SetB(nền), SetC | Q2 §3.4 |
| Update qua form (edit) | SetB, SetC, SetE | Q2 §3.5 |
| Delete qua link, real `<a>` | SetC, SetD(nền), SetF | Q2 §3.6 |

- [Question_1/question_1.md](Question_1/question_1.md) — Web API (ASP.NET Core), full CRUD + search nhiều tiêu chí, DB script tại `given_pe_trial_practice/SetH_Cinema_FullCombined/1/database.sql`.
- [Question_2/question_2.md](Question_2/question_2.md) — MVC/Razor Pages, gộp Create + Update + Delete + Search (1 & nhiều tiêu chí) trên **cùng một trang**.

**Given materials:** `given_pe_trial_practice/SetH_Cinema_FullCombined/` — chứa `1/database.sql` và `2/givenAPI` (chạy tại `http://localhost:5105`) + `2/sample.html`.

**Lưu ý:** đây là bài khó nhất trong bộ set — tương đương phải làm cả 6 set A–F cùng lúc trên 1 domain. Nên luyện A–F riêng lẻ trước, set này dùng để tổng ôn giai đoạn cuối.
