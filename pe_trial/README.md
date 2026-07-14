# PE Trial — Paper No: 2

Đề gồm 10 trang (dạng scan ảnh, không có text layer), chia làm **2 câu hỏi**, mỗi câu được tách vào một folder riêng:

- [Question_1/question_1.md](Question_1/question_1.md) — Web API (ASP.NET Core), có DB diagram.
- [Question_2/question_2.md](Question_2/question_2.md) — MVC/Razor Pages, gọi RESTful API có sẵn (GivenAPIs).

Ảnh gốc từng trang được lưu trong `images/` của mỗi folder câu hỏi (trang nào liên quan tới câu nào thì nằm trong folder đó; trang 6 — ranh giới giữa 2 câu — được lưu ở cả hai).

## Instructions chung (trang 1/10)

Please read the instructions carefully before doing the questions.

- You can use materials in your computer, notebook and text book.
- You are **NOT allowed** to use any device to share data with others.

Beside the above conditions, students must follow the following requirements:

1. The work must complete by using Visual Studio 2022++
2. The Framework must be .NET 8.0
3. **THIS PART IS VERY IMPORTANT, PLEASE READ IT CAREFULLY AND FOLLOW THE INSTRUCTIONS.**
   - You are given a database script (.sql file) in Zip file. Execute the script before doing questions.
   - **You must use the given solution.**
   - **You are not allowed to add any more libraries via NuGet Package Manager into given solution.**
   - **Submission Guideline:**
     Submit your work for each question separately. For each question, please:
     - Publish your project using the command:
       ```
       dotnet publish -c Release -o ./[QuestionNumber_StudentAccount]
       ```
       Example:
       ```
       dotnet publish -c Release -o ./Q1_trungnthe123432
       ```
     - Submit the root folder of the project into the PEA_Client application.
     - If the root folder of the project is too large, you may delete the following subfolders to reduce its size before submitting: `/bin`, `/obj`

**Just one of above requirements is violated, your work will be considered as invalid.**
