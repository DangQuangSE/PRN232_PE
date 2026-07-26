namespace Q2.Models.Dtos
{
    public class BookModelView
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? PublishDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public AuthorModelView? Author { get; set; }
        public List<TranslatorModelView> Translators { get; set; }

    }
    public class AuthorModelView
    {
        public int Id { get; set; }
        public string? FullName { get; set; } 
    }
    public class TranslatorModelView
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }
}
