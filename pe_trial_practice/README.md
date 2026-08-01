# PE Trial — Practice Set

Bộ đề luyện tập mô phỏng đúng format của `pe_trial/` gốc (Paper No. 2), dùng để luyện thêm với chủ đề dữ liệu khác và các thao tác CRUD khác nhau ở Question 2.

| Set | Chủ đề | Q1 (Web API) | Q2 thao tác chính | Ghi chú |
|---|---|---|---|---|
| [SetA_Library_Create](SetA_Library_Create/) | Thư viện sách (Author/Book) | CRUD Author + list Book | **Create** | Q2 thêm mới Book qua form |
| [SetB_Ecommerce_Update](SetB_Ecommerce_Update/) | Thương mại điện tử (Designer/Product) | CRUD Designer + list Product | **Update** | Q2 sửa Product qua form edit |
| [SetC_School_Combined](SetC_School_Combined/) | Trường học (Teacher/Course) | CRUD Teacher + list Course | **Create + Update + Delete** | Q2 làm đủ 3 thao tác trên 1 trang |
| [SetD_Ecommerce_SearchOne](SetD_Ecommerce_SearchOne/) | Thương mại điện tử | **Delete Designer** | **Search 1 tiêu chí** | Full set + GivenAPI riêng |
| [SetE_Ecommerce_SearchMulti](SetE_Ecommerce_SearchMulti/) | Thương mại điện tử | **Update Designer** | **Search nhiều tiêu chí** | Full set + GivenAPI riêng |
| [SetF_Ecommerce_Delete](SetF_Ecommerce_Delete/) | Thương mại điện tử | **Search Designer nhiều tiêu chí** | **Delete Product** | Full set + GivenAPI riêng |
| [SetG_Cinema_JwtAuth](SetG_Cinema_JwtAuth/) | Điện ảnh (Director/Movie), cùng schema với given chính thức | CRUD Director + **JWT login/protect** | **Create** | Q1 thêm bảo mật JWT (login + `[Authorize(Roles="Admin")]`), Q2 thêm mới Movie qua form |
| [SetH_Cinema_FullCombined](SetH_Cinema_FullCombined/) | Điện ảnh (Director/Movie), domain của đề thi thật gốc | **Full CRUD + Search nhiều tiêu chí** | **Create + Update + Delete + Search (1 & nhiều tiêu chí)** | Bài tổng ôn — gộp mọi thao tác của Set A–F (trừ JWT) vào 1 Q1 + 1 Q2 duy nhất |

## Cách dùng bộ đề này

Mỗi set có cấu trúc giống hệt `pe_trial/` + `given_pe_trial/` gốc, tách làm 2 phần:

```
pe_trial_practice/SetX_.../         — đề bài (giống pe_trial/)
├── README.md                       — hướng dẫn chung riêng cho set này
├── Question_1/question_1.md        — đề Web API + schema DB
└── Question_2/question_2.md        — đề MVC/Razor Pages gọi GivenAPI

given_pe_trial_practice/SetX_.../   — tài liệu/công cụ cho sẵn (giống given_pe_trial/)
├── 1/database.sql                  — script tạo DB + seed data cho Question 1, tự chạy trên SQL Server
└── 2/
    ├── givenAPI/                   — project GivenAPI đã code sẵn, chạy được ngay, KHÔNG được sửa
    └── sample.html                 — mockup HTML minh hoạ đúng quy ước id cho Question 2
```

GivenAPI của mỗi set chạy độc lập trên một port riêng (không đụng port của bài PE trial gốc 5000/5001/5100):

| Set | Port GivenAPI |
|---|---|
| SetA_Library_Create | `http://localhost:5200` |
| SetB_Ecommerce_Update | `http://localhost:5100` |
| SetC_School_Combined | `http://localhost:5202` |
| SetD_Ecommerce_SearchOne | `http://localhost:5101` |
| SetE_Ecommerce_SearchMulti | `http://localhost:5102` |
| SetF_Ecommerce_Delete | `http://localhost:5103` |
| SetG_Cinema_JwtAuth | `http://localhost:5104` |
| SetH_Cinema_FullCombined | `http://localhost:5105` |

Trước khi luyện Question 2 của set nào, chạy `dotnet run` trong `given_pe_trial_practice/SetX_.../2/givenAPI/` (project đã build/test sẵn, dữ liệu in-memory reset mỗi lần chạy lại).

Question 1 của cả 6 set đều tự đứng độc lập được — chỉ cần chạy script SQL kèm theo (`given_pe_trial_practice/SetX_.../1/database.sql`) trên SQL Server rồi code Web API như đề gốc.

## Quy tắc chung (áp dụng cho cả 6 set)

- Framework: ASP.NET Core, **.NET 8.0**, Visual Studio 2022+.
- Q1 chạy tại `http://localhost:5000`, connection string đọc từ `appsettings.json` key `ConnectionStrings:MyCnn`.
- Q2 gọi GivenAPI qua `HttpClient`, base URL đọc từ `appsettings.json` key `GivenAPIBaseUrl` (xem bảng port ở trên).
- Tất cả input/output element trong HTML của Q2 phải có `id` đúng quy ước nêu trong từng đề — đây là tiêu chí chấm tự động, sai id là mất điểm dù logic đúng.
- Không thêm NuGet package ngoài mức cần thiết tối thiểu (EF Core SqlServer + Design cho Q1; không cần gì thêm ngoài mặc định cho Q2).
- Không được sửa code trong `given_pe_trial_practice/SetX_.../2/givenAPI/` — coi như tài liệu instructor cung cấp, chỉ được gọi vào qua HTTP.
