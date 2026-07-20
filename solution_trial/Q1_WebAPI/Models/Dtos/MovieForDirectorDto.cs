using Q1_WebAPI.Models;

namespace Q1_WebAPI.Models.Dtos;

public class MovieForDirectorDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Description { get; set; }
    public string Language { get; set; } = null!;
    public int? ProducerId { get; set; }
    public int? DirectorId { get; set; }
    public string? ProducerName { get; set; }
    public string? DirectorName { get; set; }
    public List<object> Genres { get; set; } = new();
    public List<object> Stars { get; set; } = new();

    public static MovieForDirectorDto FromEntity(Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        ReleaseDate = movie.ReleaseDate,
        ReleaseYear = movie.ReleaseDate?.Year,
        Description = movie.Description,
        Language = movie.Language,
        ProducerId = movie.ProducerId,
        DirectorId = movie.DirectorId,
        ProducerName = movie.Producer?.Name,
        DirectorName = movie.Director?.FullName
    };
}
