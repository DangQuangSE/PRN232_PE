using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public partial class Star
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool? Male { get; set; }
        public DateTime? Dob { get; set; }
        public string? Description { get; set; }
        public string? Nationality { get; set; }

        // Navigation
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
