using System;
using System.Collections.Generic;

namespace givenAPI.Models;

public partial class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public string? Description { get; set; }
    public string Language { get; set; } = null!;
    public int? TeacherId { get; set; }
    public int? DepartmentId { get; set; }

    // Navigation
    public virtual Teacher? Teacher { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<Assistant> Assistants { get; set; } = new List<Assistant>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
