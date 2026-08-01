#nullable disable
using System;
using System.Collections.Generic;

namespace Q1.Models;

public partial class Director
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public bool Male { get; set; }

    public DateOnly Dob { get; set; }

    public string Nationality { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
