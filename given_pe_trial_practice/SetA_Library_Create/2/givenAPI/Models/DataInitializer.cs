using givenAPI.Models;
using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public static class DataInitializer
    {
        // 1. Authors
        public static List<Author> Authors = new List<Author>
        {
            new Author { Id = 1, FullName = "J.K. Rowling", Male = false, Dob = new DateTime(1965, 7, 31), Nationality = "England", Description = "British author, best known for the Harry Potter series." },
            new Author { Id = 2, FullName = "George R.R. Martin", Male = true, Dob = new DateTime(1948, 9, 20), Nationality = "USA", Description = "American novelist, author of A Song of Ice and Fire." },
            new Author { Id = 3, FullName = "Haruki Murakami", Male = true, Dob = new DateTime(1949, 1, 12), Nationality = "Japan", Description = "Japanese writer known for surreal, melancholic novels." },
            new Author { Id = 4, FullName = "Agatha Christie", Male = false, Dob = new DateTime(1890, 9, 15), Nationality = "England", Description = "English writer known for detective novels featuring Hercule Poirot." },
            new Author { Id = 5, FullName = "Stephen King", Male = true, Dob = new DateTime(1947, 9, 21), Nationality = "USA", Description = "American author of horror and supernatural fiction." }
        };

        // 2. Publishers
        public static List<Publisher> Publishers = new List<Publisher>
        {
            new Publisher { Id = 1, Name = "Bloomsbury" },
            new Publisher { Id = 2, Name = "Bantam Books" },
            new Publisher { Id = 3, Name = "Kodansha" },
            new Publisher { Id = 4, Name = "HarperCollins" },
            new Publisher { Id = 5, Name = "Scribner" }
        };

        // 3. Genres
        public static List<Genre> Genres = new List<Genre>
        {
            new Genre { Id = 1, Title = "Fantasy" },
            new Genre { Id = 2, Title = "Drama" },
            new Genre { Id = 3, Title = "Mystery" },
            new Genre { Id = 4, Title = "Horror" },
            new Genre { Id = 5, Title = "Romance" }
        };

        // 4. Translators (added for realism, even though question 1 showed empty translators)
        public static List<Translator> Translators = new List<Translator>
        {
            new Translator { Id = 1, FullName = "Jay Rubin", Male = true, Dob = new DateTime(1941, 3, 15), Description = "Renowned translator of Japanese literature.", Nationality = "USA" },
            new Translator { Id = 2, FullName = "Philip Gabriel", Male = true, Dob = new DateTime(1950, 5, 20), Description = "Translator of contemporary Japanese fiction.", Nationality = "USA" }
        };

        // 5. Books
        public static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone", PublishDate = new DateTime(1997, 6, 26), Description = "A young boy discovers he is a wizard.", Language = "English", PublisherId = 1, AuthorId = 1 },
            new Book { Id = 2, Title = "A Game of Thrones", PublishDate = new DateTime(1996, 8, 1), Description = "Noble families vie for control of the Iron Throne.", Language = "English", PublisherId = 2, AuthorId = 2 },
            new Book { Id = 3, Title = "Norwegian Wood", PublishDate = new DateTime(1987, 9, 4), Description = "A nostalgic story of loss and burgeoning sexuality.", Language = "Japanese", PublisherId = 3, AuthorId = 3 },
            new Book { Id = 4, Title = "Murder on the Orient Express", PublishDate = new DateTime(1934, 1, 1), Description = "Detective Poirot investigates a murder aboard a train.", Language = "English", PublisherId = 4, AuthorId = 4 },
            new Book { Id = 5, Title = "The Shining", PublishDate = new DateTime(1977, 1, 28), Description = "A family isolated in a haunted hotel.", Language = "English", PublisherId = 5, AuthorId = 5 },
            new Book { Id = 6, Title = "Kafka on the Shore", PublishDate = new DateTime(2002, 9, 12), Description = "A teenage runaway and an aging simpleton's intertwined journeys.", Language = "Japanese", PublisherId = 3, AuthorId = 3 }
        };

        // 6. BookGenres (no associations in the seed data, so empty list)
        public static List<BookGenre> BookGenres = new List<BookGenre>
        {
        };

        // 7. BookTranslators (no associations in the seed data, so empty list)
        public static List<BookTranslator> BookTranslators = new List<BookTranslator>
        {
        };

        // 8. Static constructor to wire up navigation properties
        static DataInitializer()
        {
        }
    }
}
