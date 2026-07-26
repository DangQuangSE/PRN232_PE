using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public partial class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
