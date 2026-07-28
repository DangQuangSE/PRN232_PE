using System;
using System.Collections.Generic;

namespace givenAPI.Models;

public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? LaunchDate { get; set; }
    public string? Description { get; set; }
    public string Material { get; set; } = null!;
    public int? ManufacturerId { get; set; }
    public int? DesignerId { get; set; }

    // Navigation
    public virtual Manufacturer? Manufacturer { get; set; }
    public virtual Designer? Designer { get; set; }
    public virtual ICollection<Reviewer> Reviewers { get; set; } = new List<Reviewer>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
