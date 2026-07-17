using System;
using System.Collections.Generic;

namespace givenAPI.Models;

public partial class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
