using System;
using System.Collections.Generic;

namespace givenAPI.Models;

public partial class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? PublishDate { get; set; }
    public string? Description { get; set; }
    public string Language { get; set; } = null!;
    public int? PublisherId { get; set; }
    public int? AuthorId { get; set; }

    // Navigation
    public virtual Publisher? Publisher { get; set; }
    public virtual Author? Author { get; set; }
    public virtual ICollection<Translator> Translators { get; set; } = new List<Translator>();
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
