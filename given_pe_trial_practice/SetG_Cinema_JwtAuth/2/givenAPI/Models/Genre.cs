using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public partial class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        // Navigation
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
