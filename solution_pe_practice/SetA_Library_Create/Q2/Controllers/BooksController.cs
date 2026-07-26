// Controllers/BooksController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/Books/GetBooks
        [HttpGet("GetBooks")]
        public ActionResult<List<BookResponse>> GetBooks()
        {
            var books = DataInitializer.Books;
            var authors = DataInitializer.Authors;
            var translators = DataInitializer.Translators;
            var genres = DataInitializer.Genres;
            var bookTranslators = DataInitializer.BookTranslators;
            var bookGenres = DataInitializer.BookGenres;

            List<BookResponse> result = books
                .Select(b =>
                {
                    // Find Author for each book
                    var author = authors.FirstOrDefault(a => a.Id == b.AuthorId);

                    return new BookResponse
                    {
                        Id = b.Id,
                        Title = b.Title,
                        PublishDate = b.PublishDate,
                        Description = b.Description,
                        Language = b.Language,
                        PublisherId = b.PublisherId,
                        AuthorId = b.AuthorId,

                        Author = author is null ? null : new BookResponse.AuthorInfo
                        {
                            Id = author.Id,
                            FullName = author.FullName,
                            Male = author.Male,
                            Dob = author.Dob,
                            Nationality = author.Nationality,
                            Description = author.Description
                        },

                        Translators = bookTranslators
                            .Where(bt => bt.BookId == b.Id)
                            .Select(bt =>
                            {
                                var t = translators.First(tr => tr.Id == bt.TranslatorId);
                                return new BookResponse.TranslatorInfo
                                {
                                    Id = t.Id,
                                    FullName = t.FullName,
                                    Male = t.Male,
                                    Dob = t.Dob,
                                    Description = t.Description,
                                    Nationality = t.Nationality
                                };
                            })
                            .ToList(),

                        Genres = bookGenres
                            .Where(bg => bg.BookId == b.Id)
                            .Select(bg =>
                            {
                                var g = genres.First(ge => ge.Id == bg.GenreId);
                                return new BookResponse.GenreInfo
                                {
                                    Id = g.Id,
                                    Title = g.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/Books/GetBooksByAuthorId/{authorId}
        [HttpGet("GetBooksByAuthorId/{authorId}")]
        public ActionResult<List<BookResponse>> GetBooksByAuthorId(int authorId)
        {
            var books = DataInitializer.Books.Where(b => b.AuthorId == authorId);
            var authors = DataInitializer.Authors;
            var translators = DataInitializer.Translators;
            var genres = DataInitializer.Genres;
            var bookTranslators = DataInitializer.BookTranslators;
            var bookGenres = DataInitializer.BookGenres;

            List<BookResponse> result = books
                .Select(b =>
                {
                    var author = authors.FirstOrDefault(a => a.Id == b.AuthorId);

                    return new BookResponse
                    {
                        Id = b.Id,
                        Title = b.Title,
                        PublishDate = b.PublishDate,
                        Description = b.Description,
                        Language = b.Language,
                        PublisherId = b.PublisherId,
                        AuthorId = b.AuthorId,

                        Author = author is null ? null : new BookResponse.AuthorInfo
                        {
                            Id = author.Id,
                            FullName = author.FullName,
                            Male = author.Male,
                            Dob = author.Dob,
                            Nationality = author.Nationality,
                            Description = author.Description
                        },

                        Translators = bookTranslators
                            .Where(bt => bt.BookId == b.Id)
                            .Select(bt =>
                            {
                                var t = translators.First(tr => tr.Id == bt.TranslatorId);
                                return new BookResponse.TranslatorInfo
                                {
                                    Id = t.Id,
                                    FullName = t.FullName,
                                    Male = t.Male,
                                    Dob = t.Dob,
                                    Description = t.Description,
                                    Nationality = t.Nationality
                                };
                            })
                            .ToList(),

                        Genres = bookGenres
                            .Where(bg => bg.BookId == b.Id)
                            .Select(bg =>
                            {
                                var g = genres.First(ge => ge.Id == bg.GenreId);
                                return new BookResponse.GenreInfo
                                {
                                    Id = g.Id,
                                    Title = g.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // POST: api/Books/CreateBook
        [HttpPost("CreateBook")]
        public ActionResult<BookResponse> CreateBook([FromBody] CreateBookRequest request)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Language))
            {
                return BadRequest(new { Message = "Title and Language are required." });
            }

            // Check if authorId is valid
            var author = DataInitializer.Authors.FirstOrDefault(a => a.Id == request.AuthorId);
            if (author is null)
            {
                return BadRequest(new { Message = $"Author with Id {request.AuthorId} not found." });
            }

            // Generate new Id (find max and increment)
            var newId = DataInitializer.Books.Count > 0
                ? DataInitializer.Books.Max(b => b.Id) + 1
                : 1;

            // Create new book
            var newBook = new Book
            {
                Id = newId,
                Title = request.Title,
                PublishDate = request.PublishDate,
                Description = request.Description,
                Language = request.Language,
                PublisherId = request.PublisherId,
                AuthorId = request.AuthorId
            };

            DataInitializer.Books.Add(newBook);

            // Build response
            var response = new BookResponse
            {
                Id = newBook.Id,
                Title = newBook.Title,
                PublishDate = newBook.PublishDate,
                Description = newBook.Description,
                Language = newBook.Language,
                PublisherId = newBook.PublisherId,
                AuthorId = newBook.AuthorId,

                Author = new BookResponse.AuthorInfo
                {
                    Id = author.Id,
                    FullName = author.FullName,
                    Male = author.Male,
                    Dob = author.Dob,
                    Nationality = author.Nationality,
                    Description = author.Description
                },

                Translators = new List<BookResponse.TranslatorInfo>(),
                Genres = new List<BookResponse.GenreInfo>()
            };

            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, response);
        }

        // Request DTO
        public class CreateBookRequest
        {
            public string Title { get; set; } = null!;
            public DateTime? PublishDate { get; set; }
            public string? Description { get; set; }
            public string Language { get; set; } = null!;
            public int? PublisherId { get; set; }
            public int AuthorId { get; set; }
        }
    }
}
