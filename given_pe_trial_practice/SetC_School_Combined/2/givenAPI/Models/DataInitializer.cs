using givenAPI.Models;
using givenAPI.Models.Responses;
using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public static class DataInitializer
    {
        // 1. Teachers
        public static List<Teacher> Teachers = new List<Teacher>
        {
            new Teacher { Id = 1, FullName = "Richard Feynman", Male = true, Dob = new DateTime(1918,5,11), Nationality = "USA", Description = "Theoretical physicist known for work in quantum electrodynamics." },
            new Teacher { Id = 2, FullName = "Marie Curie", Male = false, Dob = new DateTime(1867,11,7), Nationality = "Poland", Description = "Physicist and chemist, pioneer of radioactivity research." },
            new Teacher { Id = 3, FullName = "Alan Turing", Male = true, Dob = new DateTime(1912,6,23), Nationality = "England", Description = "Mathematician and computer scientist, father of theoretical computer science." },
            new Teacher { Id = 4, FullName = "Ada Lovelace", Male = false, Dob = new DateTime(1815,12,10), Nationality = "England", Description = "Mathematician, wrote the first algorithm for a computing machine." },
            new Teacher { Id = 5, FullName = "Carl Sagan", Male = true, Dob = new DateTime(1934,11,9), Nationality = "USA", Description = "Astronomer and science communicator." }
        };

        // 2. Departments
        public static List<Department> Departments = new List<Department>
        {
            new Department { Id = 1, Name = "Physics" },
            new Department { Id = 2, Name = "Chemistry" },
            new Department { Id = 3, Name = "Computer Science" },
            new Department { Id = 4, Name = "Mathematics" },
            new Department { Id = 5, Name = "Astronomy" }
        };

        // 3. Courses
        public static List<Course> Courses = new List<Course>
        {
            new Course { Id = 1, Title = "Quantum Mechanics 101", StartDate = new DateTime(2024,9,1), Description = "Introduction to quantum theory.", Language = "English", DepartmentId = 1, TeacherId = 1 },
            new Course { Id = 2, Title = "Radioactivity Fundamentals", StartDate = new DateTime(2024,9,5), Description = "Basics of radioactive decay and applications.", Language = "English", DepartmentId = 2, TeacherId = 2 },
            new Course { Id = 3, Title = "Introduction to Computing", StartDate = new DateTime(2024,9,10), Description = "History and theory of computation.", Language = "English", DepartmentId = 3, TeacherId = 3 },
            new Course { Id = 4, Title = "Algorithms and Machines", StartDate = new DateTime(2025,1,15), Description = "Turing machines and algorithmic thinking.", Language = "English", DepartmentId = 3, TeacherId = 3 },
            new Course { Id = 5, Title = "Analytical Engine Programming", StartDate = new DateTime(2024,9,12), Description = "Programming concepts using historical machines.", Language = "English", DepartmentId = 4, TeacherId = 4 },
            new Course { Id = 6, Title = "Cosmos and Beyond", StartDate = new DateTime(2024,9,20), Description = "A tour of the universe.", Language = "English", DepartmentId = 5, TeacherId = 5 }
        };

        // 4. Assistants
        public static List<Assistant> Assistants = new List<Assistant>
        {
            new Assistant { Id = 1, FullName = "John Smith", Male = true, Dob = new DateTime(1995,3,15), Description = "Graduate assistant specializing in quantum mechanics.", Nationality = "USA" },
            new Assistant { Id = 2, FullName = "Sarah Johnson", Male = false, Dob = new DateTime(1996,7,22), Description = "Research assistant in computational theory.", Nationality = "Canada" },
            new Assistant { Id = 3, FullName = "Michael Chen", Male = true, Dob = new DateTime(1994,11,8), Description = "Lab assistant for experimental physics.", Nationality = "USA" }
        };

        // 5. Tags
        public static List<Tag> Tags = new List<Tag>
        {
            new Tag { Id = 1, Title = "Science" },
            new Tag { Id = 2, Title = "Theory" },
            new Tag { Id = 3, Title = "Lab" },
            new Tag { Id = 4, Title = "History" },
            new Tag { Id = 5, Title = "Advanced" }
        };

        // 6. CourseAssistants
        public static List<CourseAssistant> CourseAssistants = new List<CourseAssistant>
        {
            new CourseAssistant { CourseId = 1, AssistantId = 1 },
            new CourseAssistant { CourseId = 2, AssistantId = 1 },
            new CourseAssistant { CourseId = 3, AssistantId = 2 },
            new CourseAssistant { CourseId = 4, AssistantId = 2 },
            new CourseAssistant { CourseId = 6, AssistantId = 3 }
        };

        // 7. CourseTags
        public static List<CourseTag> CourseTags = new List<CourseTag>
        {
            new CourseTag { CourseId = 1, TagId = 1 },
            new CourseTag { CourseId = 1, TagId = 2 },
            new CourseTag { CourseId = 2, TagId = 1 },
            new CourseTag { CourseId = 2, TagId = 3 },
            new CourseTag { CourseId = 3, TagId = 1 },
            new CourseTag { CourseId = 3, TagId = 4 },
            new CourseTag { CourseId = 4, TagId = 2 },
            new CourseTag { CourseId = 4, TagId = 5 },
            new CourseTag { CourseId = 5, TagId = 4 },
            new CourseTag { CourseId = 6, TagId = 1 }
        };

        // Static constructor to wire up navigation properties
        static DataInitializer()
        {
        }
    }
}
