using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? PublishDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? PublisherId { get; set; }
        public int? AuthorId { get; set; }

        public AuthorInfo? Author { get; set; }

        public List<TranslatorInfo> Translators { get; set; } = new();

        public List<GenreInfo> Genres { get; set; } = new();

        public class AuthorInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool Male { get; set; }
            public DateTime Dob { get; set; }
            public string Nationality { get; set; } = null!;
            public string Description { get; set; } = null!;
        }

        public class TranslatorInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool? Male { get; set; }
            public DateTime? Dob { get; set; }
            public string? Description { get; set; }
            public string? Nationality { get; set; }
        }

        public class GenreInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
        }
    }
}
