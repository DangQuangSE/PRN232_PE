using Q1_WebAPI.Models;

namespace Q1_WebAPI.Models.Dtos;

public class DirectorWithMoviesDto : DirectorDto
{
    public List<MovieForDirectorDto> Movies { get; set; } = new();

    public static new DirectorWithMoviesDto FromEntity(Director director)
    {
        var dto = DirectorDto.FromEntity(director);
        return new DirectorWithMoviesDto
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Gender = dto.Gender,
            Dob = dto.Dob,
            DobString = dto.DobString,
            Nationality = dto.Nationality,
            Description = dto.Description,
            Movies = director.Movies.Select(MovieForDirectorDto.FromEntity).ToList()
        };
    }
}
