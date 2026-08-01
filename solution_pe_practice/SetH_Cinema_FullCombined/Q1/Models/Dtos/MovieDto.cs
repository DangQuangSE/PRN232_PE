namespace Q1.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleaseYear { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int? ProducerId { get; set; }
        public int? DirectorId { get; set; }
        public string ProducerName { get; set; }
        public string DirectorName { get; set; }
        public List<object> Genres { get; set; } = new();
        public List<object> Stars { get; set; } = new();
    }
}
