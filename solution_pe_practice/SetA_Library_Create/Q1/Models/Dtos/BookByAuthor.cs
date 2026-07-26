namespace Q1.Models.Dtos
{
    public class BookByAuthor
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? PublishDate { get; set; }
        public int PublishYear { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }
        public int? PublisherId { get; set; }

        public int? AuthorId { get; set; }
        public string? PublisherName { get; set; }
        public string? AuthorName { get; set; }
        public List<Genre> Geners { get; set; } = [];
        public List<Translator> Translators { get; set; } = [];
    }
}
