using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public static class AuthorMapper
    {
        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Gender = author.Male ? "Male" : "Female",
                Dob = author.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = author.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
                Nationality = author.Nationality,
                Description = author.Description,
            };
        }

        public static AuthorWithBookDTO ToAuthorWithBookDto(this Author author)
        {
            return new AuthorWithBookDTO
            {
                Id = author.Id,
                FullName = author.FullName,
                Gender = author.Male ? "Male" : "Female",
                Dob = author.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = author.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
                Nationality = author.Nationality,
                Description = author.Description,
                Books = author.Books?.Select(b => b.ToBookByAuthor()).ToList() ?? new List<BookByAuthor>(),
            };
        }

        public static BookByAuthor ToBookByAuthor(this Book book)
        {
            return new BookByAuthor
            {
                Id = book.Id,
                Title = book.Title,
                PublishDate = book.PublishDate.HasValue ? book.PublishDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                PublishYear = book.PublishDate.HasValue ? book.PublishDate.Value.Year : 0,
                Description = book.Description,
                Language = book.Language,
                PublisherId = book.PublisherId,
                AuthorId = book.AuthorId,
                PublisherName = book.Publisher != null ? book.Publisher.Name : null,
                AuthorName = book.Author != null ? book.Author.FullName : null,
            };
        }
    }
}
