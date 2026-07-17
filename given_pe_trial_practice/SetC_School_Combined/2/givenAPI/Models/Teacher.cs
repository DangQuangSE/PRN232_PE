using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool Male { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; } = null!;
        public string Description { get; set; } = null!;

        // Navigation
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
