namespace Q1.Models.Dtos
{
    public class DirectorWithMoviesDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string DobString { get; set; }
        public string Nationality { get; set; }
        public string Description { get; set; }
        public List<MovieDto> Movies { get; set; } = new();
    }
}
