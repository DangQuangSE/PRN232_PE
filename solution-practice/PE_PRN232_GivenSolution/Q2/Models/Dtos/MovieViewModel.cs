namespace Q2.Models.Dtos
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public DirectorViewModel? Director { get; set; }

        public List<StarViewModel> Stars { get; set; } = [];
    }
    public class DirectorViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }
    public class StarViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }
}
