namespace Q1_WebAPI.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string Language { get; set; } = null!;
    public int? ProducerId { get; set; }
    public int? DirectorId { get; set; }

    public Producer? Producer { get; set; }
    public Director? Director { get; set; }
}
