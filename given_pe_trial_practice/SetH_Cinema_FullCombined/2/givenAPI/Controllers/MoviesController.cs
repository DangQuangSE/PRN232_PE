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

        // GET: api/Movies/GetMovieById/{id}
        [HttpGet("GetMovieById/{id}")]
        public ActionResult<MovieResponse> GetMovieById(int id)
        {
            var movie = DataInitializer.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound(new { Message = $"Movie with Id = {id} not found." });

            return Ok(MovieToResponse(movie));
        }

        // GET: api/Movies/SearchMovies?title=&language=&directorId=&fromYear=&toYear=
        [HttpGet("SearchMovies")]
        public ActionResult<List<MovieResponse>> SearchMovies(
            [FromQuery] string? title,
            [FromQuery] string? language,
            [FromQuery] int? directorId,
            [FromQuery] int? fromYear,
            [FromQuery] int? toYear)
        {
            var movies = DataInitializer.Movies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(title))
                movies = movies.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(language))
                movies = movies.Where(m => m.Language.Contains(language, StringComparison.OrdinalIgnoreCase));

            if (directorId.HasValue)
                movies = movies.Where(m => m.DirectorId == directorId.Value);

            if (fromYear.HasValue)
                movies = movies.Where(m => m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year >= fromYear.Value);

            if (toYear.HasValue)
                movies = movies.Where(m => m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year <= toYear.Value);

            var result = movies.Select(m => MovieToResponse(m)).ToList();
            return Ok(result);
        }

        // POST: api/Movies/CreateMovie
        [HttpPost("CreateMovie")]
        public ActionResult<MovieResponse> CreateMovie([FromBody] CreateMovieRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Language))
            {
                return BadRequest(new { Message = "Title and Language are required." });
            }

            var director = DataInitializer.Directors.FirstOrDefault(d => d.Id == request.DirectorId);
            if (director is null)
            {
                return BadRequest(new { Message = $"Director with Id {request.DirectorId} not found." });
            }

            var newId = DataInitializer.Movies.Count > 0
                ? DataInitializer.Movies.Max(m => m.Id) + 1
                : 1;

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

            return CreatedAtAction(nameof(GetMovies), new { id = newMovie.Id }, MovieToResponse(newMovie));
        }

        // PUT: api/Movies/UpdateMovie/{id}
        [HttpPut("UpdateMovie/{id}")]
        public ActionResult<MovieResponse> UpdateMovie(int id, [FromBody] UpdateMovieRequest request)
        {
            var movie = DataInitializer.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound(new { Message = $"Movie with Id = {id} not found." });

            movie.Title = request.Title;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Description = request.Description;
            movie.Language = request.Language;
            movie.DirectorId = request.DirectorId;

            return Ok(MovieToResponse(movie));
        }

        // DELETE: api/Movies/DeleteMovie/{id}
        [HttpDelete("DeleteMovie/{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = DataInitializer.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound(new { Message = $"Movie with Id = {id} not found." });

            DataInitializer.Movies.Remove(movie);
            return NoContent();
        }

        // Request DTOs
        public class CreateMovieRequest
        {
            public string Title { get; set; } = null!;
            public DateTime? ReleaseDate { get; set; }
            public string? Description { get; set; }
            public string Language { get; set; } = null!;
            public int? ProducerId { get; set; }
            public int DirectorId { get; set; }
        }

        public class UpdateMovieRequest
        {
            public string Title { get; set; } = null!;
            public DateTime? ReleaseDate { get; set; }
            public string? Description { get; set; }
            public string Language { get; set; } = null!;
            public int? DirectorId { get; set; }
        }
    }
}
