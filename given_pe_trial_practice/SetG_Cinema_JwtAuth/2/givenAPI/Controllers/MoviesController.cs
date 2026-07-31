// Controllers/MoviesController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // Helper method to convert Movie to MovieResponse
        private MovieResponse MovieToResponse(Movie m)
        {
            var directors = DataInitializer.Directors;
            var genres = DataInitializer.Genres;
            var stars = DataInitializer.Stars;
            var movieGenres = DataInitializer.MovieGenres;
            var movieStars = DataInitializer.MovieStars;

            var director = directors.FirstOrDefault(d => d.Id == m.DirectorId);

            return new MovieResponse
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                Description = m.Description,
                Language = m.Language,
                ProducerId = m.ProducerId,
                DirectorId = m.DirectorId,

                Director = director is null ? null : new MovieResponse.DirectorInfo
                {
                    Id = director.Id,
                    FullName = director.FullName,
                    Male = director.Male,
                    Dob = director.Dob,
                    Nationality = director.Nationality,
                    Description = director.Description
                },

                Genres = movieGenres
                    .Where(mg => mg.MovieId == m.Id)
                    .Select(mg =>
                    {
                        var g = genres.First(ge => ge.Id == mg.GenreId);
                        return new MovieResponse.GenreInfo
                        {
                            Id = g.Id,
                            Title = g.Title
                        };
                    })
                    .ToList(),

                Stars = movieStars
                    .Where(ms => ms.MovieId == m.Id)
                    .Select(ms =>
                    {
                        var s = stars.First(st => st.Id == ms.StarId);
                        return new MovieResponse.StarInfo
                        {
                            Id = s.Id,
                            FullName = s.FullName,
                            Male = s.Male,
                            Dob = s.Dob,
                            Description = s.Description,
                            Nationality = s.Nationality
                        };
                    })
                    .ToList()
            };
        }

        // GET: api/Movies/GetMovies
        [HttpGet("GetMovies")]
        public ActionResult<List<MovieResponse>> GetMovies()
        {
            var movies = DataInitializer.Movies;
            var result = movies.Select(m => MovieToResponse(m)).ToList();
            return Ok(result);
        }

        // GET: api/Movies/GetMoviesByDirectorId/{directorId}
        [HttpGet("GetMoviesByDirectorId/{directorId}")]
        public ActionResult<List<MovieResponse>> GetMoviesByDirectorId(int directorId)
        {
            var movies = DataInitializer.Movies.Where(m => m.DirectorId == directorId);
            var result = movies.Select(m => MovieToResponse(m)).ToList();
            return Ok(result);
        }

        // POST: api/Movies/CreateMovie
        [HttpPost("CreateMovie")]
        public ActionResult<MovieResponse> CreateMovie([FromBody] CreateMovieRequest request)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Language))
            {
                return BadRequest(new { Message = "Title and Language are required." });
            }

            // Check if directorId is valid
            var director = DataInitializer.Directors.FirstOrDefault(d => d.Id == request.DirectorId);
            if (director is null)
            {
                return BadRequest(new { Message = $"Director with Id {request.DirectorId} not found." });
            }

            // Generate new Id (find max and increment)
            var newId = DataInitializer.Movies.Count > 0
                ? DataInitializer.Movies.Max(m => m.Id) + 1
                : 1;

            // Create new movie
            var newMovie = new Movie
            {
                Id = newId,
                Title = request.Title,
                ReleaseDate = request.ReleaseDate,
                Description = request.Description,
                Language = request.Language,
                ProducerId = request.ProducerId,
                DirectorId = request.DirectorId
            };

            DataInitializer.Movies.Add(newMovie);

            // Build response
            var response = new MovieResponse
            {
                Id = newMovie.Id,
                Title = newMovie.Title,
                ReleaseDate = newMovie.ReleaseDate,
                Description = newMovie.Description,
                Language = newMovie.Language,
                ProducerId = newMovie.ProducerId,
                DirectorId = newMovie.DirectorId,

                Director = new MovieResponse.DirectorInfo
                {
                    Id = director.Id,
                    FullName = director.FullName,
                    Male = director.Male,
                    Dob = director.Dob,
                    Nationality = director.Nationality,
                    Description = director.Description
                },

                Genres = new List<MovieResponse.GenreInfo>(),
                Stars = new List<MovieResponse.StarInfo>()
            };

            return CreatedAtAction(nameof(GetMovies), new { id = newMovie.Id }, response);
        }

        // Request DTO
        public class CreateMovieRequest
        {
            public string Title { get; set; } = null!;
            public DateTime? ReleaseDate { get; set; }
            public string? Description { get; set; }
            public string Language { get; set; } = null!;
            public int? ProducerId { get; set; }
            public int DirectorId { get; set; }
        }
    }
}
