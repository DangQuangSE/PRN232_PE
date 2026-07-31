using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public static class DirectorMapper
    {
        public static DirectorDto ToDirectorDto(this Director director)
        {
            return new DirectorDto
            {
                Id = director.Id,
                FullName = director.FullName,
                Gender = director.Male ? "Male" : "Female",
                Dob = director.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = director.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
                Nationality = director.Nationality,
                Description = director.Description,
            };
        }

        public static DirectorWithMoviesDto ToDirectorWithMoviesDto(this Director director)
        {
            return new DirectorWithMoviesDto
            {
                Id = director.Id,
                FullName = director.FullName,
                Gender = director.Male ? "Male" : "Female",
                Dob = director.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = director.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
                Nationality = director.Nationality,
                Description = director.Description,
                Movies = director.Movies?.Select(m => m.ToMovieDto()).ToList() ?? new List<MovieDto>(),
            };
        }

        public static MovieDto ToMovieDto(this Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate.HasValue ? movie.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                ReleaseYear = movie.ReleaseDate.HasValue ? (int?)movie.ReleaseDate.Value.Year : null,
                Description = movie.Description,
                Language = movie.Language,
                ProducerId = movie.ProducerId,
                DirectorId = movie.DirectorId,
                ProducerName = movie.Producer != null ? movie.Producer.Name : null,
                DirectorName = movie.Director != null ? movie.Director.FullName : null,
                Genres = new List<object>(),
                Stars = new List<object>(),
            };
        }
    }
}
