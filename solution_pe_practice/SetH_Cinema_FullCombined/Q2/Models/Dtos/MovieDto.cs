namespace Q2.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public DirectorDto? Director { get; set; }
        public List<StarDto> Stars { get; set; } = [];
    }

    public class DirectorDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }

    public class StarDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }
}
