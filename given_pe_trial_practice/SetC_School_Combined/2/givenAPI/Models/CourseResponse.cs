// Models/CourseResponse.cs
using System;
using System.Collections.Generic;

namespace givenAPI.Models.Responses
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? TeacherId { get; set; }
        public int? DepartmentId { get; set; }

        public TeacherInfo? Teacher { get; set; }

        public List<AssistantInfo> Assistants { get; set; } = new();

        public List<TagInfo> Tags { get; set; } = new();

        public class TeacherInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool Male { get; set; }
            public DateTime Dob { get; set; }
            public string Nationality { get; set; } = null!;
            public string Description { get; set; } = null!;
        }

        public class AssistantInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool? Male { get; set; }
            public DateTime? Dob { get; set; }
            public string? Description { get; set; }
            public string? Nationality { get; set; }
        }

        public class TagInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
        }
    }
}
