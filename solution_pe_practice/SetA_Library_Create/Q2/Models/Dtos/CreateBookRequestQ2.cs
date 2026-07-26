namespace Q2.Models.Dtos
{
    public class CreateBookRequestQ2
    {
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        public int AuthorId { get; set; }
    }
}
