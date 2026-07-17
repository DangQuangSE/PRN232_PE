// Controllers/CoursesController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;
using givenAPI.Models.Responses;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        // GET: api/Courses/GetCourses
        [HttpGet("GetCourses")]
        public ActionResult<List<CourseResponse>> GetCourses()
        {
            var courses = DataInitializer.Courses;
            var teachers = DataInitializer.Teachers;
            var assistants = DataInitializer.Assistants;
            var tags = DataInitializer.Tags;
            var courseAssistants = DataInitializer.CourseAssistants;
            var courseTags = DataInitializer.CourseTags;

            List<CourseResponse> result = courses
                .Select(c =>
                {
                    // Find Teacher for this course
                    var teacher = teachers.FirstOrDefault(t => t.Id == c.TeacherId);

                    return new CourseResponse
                    {
                        Id = c.Id,
                        Title = c.Title,
                        StartDate = c.StartDate,
                        Description = c.Description,
                        Language = c.Language,
                        TeacherId = c.TeacherId,
                        DepartmentId = c.DepartmentId,

                        Teacher = teacher is null ? null : new CourseResponse.TeacherInfo
                        {
                            Id = teacher.Id,
                            FullName = teacher.FullName,
                            Male = teacher.Male,
                            Dob = teacher.Dob,
                            Nationality = teacher.Nationality,
                            Description = teacher.Description
                        },

                        Assistants = courseAssistants
                            .Where(ca => ca.CourseId == c.Id)
                            .Select(ca =>
                            {
                                var a = assistants.First(ast => ast.Id == ca.AssistantId);
                                return new CourseResponse.AssistantInfo
                                {
                                    Id = a.Id,
                                    FullName = a.FullName,
                                    Male = a.Male,
                                    Dob = a.Dob,
                                    Description = a.Description,
                                    Nationality = a.Nationality
                                };
                            })
                            .ToList(),

                        Tags = courseTags
                            .Where(ct => ct.CourseId == c.Id)
                            .Select(ct =>
                            {
                                var t = tags.First(tg => tg.Id == ct.TagId);
                                return new CourseResponse.TagInfo
                                {
                                    Id = t.Id,
                                    Title = t.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/Courses/GetCoursesByTeacherId/{teacherId}
        [HttpGet("GetCoursesByTeacherId/{teacherId}")]
        public ActionResult<List<CourseResponse>> GetCoursesByTeacherId(int teacherId)
        {
            var courses = DataInitializer.Courses.Where(c => c.TeacherId == teacherId);
            var teachers = DataInitializer.Teachers;
            var assistants = DataInitializer.Assistants;
            var tags = DataInitializer.Tags;
            var courseAssistants = DataInitializer.CourseAssistants;
            var courseTags = DataInitializer.CourseTags;

            List<CourseResponse> result = courses
                .Select(c =>
                {
                    var teacher = teachers.FirstOrDefault(t => t.Id == c.TeacherId);

                    return new CourseResponse
                    {
                        Id = c.Id,
                        Title = c.Title,
                        StartDate = c.StartDate,
                        Description = c.Description,
                        Language = c.Language,
                        TeacherId = c.TeacherId,
                        DepartmentId = c.DepartmentId,

                        Teacher = teacher is null ? null : new CourseResponse.TeacherInfo
                        {
                            Id = teacher.Id,
                            FullName = teacher.FullName,
                            Male = teacher.Male,
                            Dob = teacher.Dob,
                            Nationality = teacher.Nationality,
                            Description = teacher.Description
                        },

                        Assistants = courseAssistants
                            .Where(ca => ca.CourseId == c.Id)
                            .Select(ca =>
                            {
                                var a = assistants.First(ast => ast.Id == ca.AssistantId);
                                return new CourseResponse.AssistantInfo
                                {
                                    Id = a.Id,
                                    FullName = a.FullName,
                                    Male = a.Male,
                                    Dob = a.Dob,
                                    Description = a.Description,
                                    Nationality = a.Nationality
                                };
                            })
                            .ToList(),

                        Tags = courseTags
                            .Where(ct => ct.CourseId == c.Id)
                            .Select(ct =>
                            {
                                var t = tags.First(tg => tg.Id == ct.TagId);
                                return new CourseResponse.TagInfo
                                {
                                    Id = t.Id,
                                    Title = t.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/Courses/GetCourseById/{id}
        [HttpGet("GetCourseById/{id}")]
        public ActionResult<CourseResponse> GetCourseById(int id)
        {
            var course = DataInitializer.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { Message = $"Course with Id = {id} not found." });

            var teachers = DataInitializer.Teachers;
            var assistants = DataInitializer.Assistants;
            var tags = DataInitializer.Tags;
            var courseAssistants = DataInitializer.CourseAssistants;
            var courseTags = DataInitializer.CourseTags;

            var teacher = teachers.FirstOrDefault(t => t.Id == course.TeacherId);

            var result = new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                StartDate = course.StartDate,
                Description = course.Description,
                Language = course.Language,
                TeacherId = course.TeacherId,
                DepartmentId = course.DepartmentId,

                Teacher = teacher is null ? null : new CourseResponse.TeacherInfo
                {
                    Id = teacher.Id,
                    FullName = teacher.FullName,
                    Male = teacher.Male,
                    Dob = teacher.Dob,
                    Nationality = teacher.Nationality,
                    Description = teacher.Description
                },

                Assistants = courseAssistants
                    .Where(ca => ca.CourseId == course.Id)
                    .Select(ca =>
                    {
                        var a = assistants.First(ast => ast.Id == ca.AssistantId);
                        return new CourseResponse.AssistantInfo
                        {
                            Id = a.Id,
                            FullName = a.FullName,
                            Male = a.Male,
                            Dob = a.Dob,
                            Description = a.Description,
                            Nationality = a.Nationality
                        };
                    })
                    .ToList(),

                Tags = courseTags
                    .Where(ct => ct.CourseId == course.Id)
                    .Select(ct =>
                    {
                        var t = tags.First(tg => tg.Id == ct.TagId);
                        return new CourseResponse.TagInfo
                        {
                            Id = t.Id,
                            Title = t.Title
                        };
                    })
                    .ToList()
            };

            return Ok(result);
        }

        // POST: api/Courses/CreateCourse
        [HttpPost("CreateCourse")]
        public ActionResult<CourseResponse> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Language))
                return BadRequest(new { Message = "Title and Language are required." });

            // Find next Id
            int newId = DataInitializer.Courses.Count > 0 ? DataInitializer.Courses.Max(c => c.Id) + 1 : 1;

            var newCourse = new Course
            {
                Id = newId,
                Title = request.Title,
                StartDate = request.StartDate,
                Description = request.Description,
                Language = request.Language,
                TeacherId = request.TeacherId
            };

            DataInitializer.Courses.Add(newCourse);

            // Return the created course response
            var teachers = DataInitializer.Teachers;
            var assistants = DataInitializer.Assistants;
            var tags = DataInitializer.Tags;
            var courseAssistants = DataInitializer.CourseAssistants;
            var courseTags = DataInitializer.CourseTags;

            var teacher = teachers.FirstOrDefault(t => t.Id == newCourse.TeacherId);

            var response = new CourseResponse
            {
                Id = newCourse.Id,
                Title = newCourse.Title,
                StartDate = newCourse.StartDate,
                Description = newCourse.Description,
                Language = newCourse.Language,
                TeacherId = newCourse.TeacherId,
                DepartmentId = newCourse.DepartmentId,

                Teacher = teacher is null ? null : new CourseResponse.TeacherInfo
                {
                    Id = teacher.Id,
                    FullName = teacher.FullName,
                    Male = teacher.Male,
                    Dob = teacher.Dob,
                    Nationality = teacher.Nationality,
                    Description = teacher.Description
                },

                Assistants = courseAssistants
                    .Where(ca => ca.CourseId == newCourse.Id)
                    .Select(ca =>
                    {
                        var a = assistants.First(ast => ast.Id == ca.AssistantId);
                        return new CourseResponse.AssistantInfo
                        {
                            Id = a.Id,
                            FullName = a.FullName,
                            Male = a.Male,
                            Dob = a.Dob,
                            Description = a.Description,
                            Nationality = a.Nationality
                        };
                    })
                    .ToList(),

                Tags = courseTags
                    .Where(ct => ct.CourseId == newCourse.Id)
                    .Select(ct =>
                    {
                        var t = tags.First(tg => tg.Id == ct.TagId);
                        return new CourseResponse.TagInfo
                        {
                            Id = t.Id,
                            Title = t.Title
                        };
                    })
                    .ToList()
            };

            return CreatedAtAction(nameof(GetCourseById), new { id = newCourse.Id }, response);
        }

        // PUT: api/Courses/UpdateCourse/{id}
        [HttpPut("UpdateCourse/{id}")]
        public ActionResult<CourseResponse> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            var course = DataInitializer.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { Message = $"Course with Id = {id} not found." });

            if (request == null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Language))
                return BadRequest(new { Message = "Title and Language are required." });

            // Update the course
            course.Title = request.Title;
            course.StartDate = request.StartDate;
            course.Description = request.Description;
            course.Language = request.Language;
            course.TeacherId = request.TeacherId;

            // Return the updated course response
            var teachers = DataInitializer.Teachers;
            var assistants = DataInitializer.Assistants;
            var tags = DataInitializer.Tags;
            var courseAssistants = DataInitializer.CourseAssistants;
            var courseTags = DataInitializer.CourseTags;

            var teacher = teachers.FirstOrDefault(t => t.Id == course.TeacherId);

            var response = new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                StartDate = course.StartDate,
                Description = course.Description,
                Language = course.Language,
                TeacherId = course.TeacherId,
                DepartmentId = course.DepartmentId,

                Teacher = teacher is null ? null : new CourseResponse.TeacherInfo
                {
                    Id = teacher.Id,
                    FullName = teacher.FullName,
                    Male = teacher.Male,
                    Dob = teacher.Dob,
                    Nationality = teacher.Nationality,
                    Description = teacher.Description
                },

                Assistants = courseAssistants
                    .Where(ca => ca.CourseId == course.Id)
                    .Select(ca =>
                    {
                        var a = assistants.First(ast => ast.Id == ca.AssistantId);
                        return new CourseResponse.AssistantInfo
                        {
                            Id = a.Id,
                            FullName = a.FullName,
                            Male = a.Male,
                            Dob = a.Dob,
                            Description = a.Description,
                            Nationality = a.Nationality
                        };
                    })
                    .ToList(),

                Tags = courseTags
                    .Where(ct => ct.CourseId == course.Id)
                    .Select(ct =>
                    {
                        var t = tags.First(tg => tg.Id == ct.TagId);
                        return new CourseResponse.TagInfo
                        {
                            Id = t.Id,
                            Title = t.Title
                        };
                    })
                    .ToList()
            };

            return Ok(response);
        }

        // DELETE: api/Courses/DeleteCourse/{id}
        [HttpDelete("DeleteCourse/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = DataInitializer.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound(new { Message = $"Course with Id = {id} not found." });

            DataInitializer.CourseAssistants.RemoveAll(ca => ca.CourseId == id);
            DataInitializer.CourseTags.RemoveAll(ct => ct.CourseId == id);
            DataInitializer.Courses.Remove(course);

            return NoContent();
        }
    }

    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? TeacherId { get; set; }
    }

    public class UpdateCourseRequest
    {
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? TeacherId { get; set; }
    }
}
