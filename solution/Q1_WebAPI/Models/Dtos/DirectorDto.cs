using Q1_WebAPI.Models;

namespace Q1_WebAPI.Models.Dtos;

public class DirectorDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateTime Dob { get; set; }
    public string DobString { get; set; } = null!;
    public string Nationality { get; set; } = null!;
    public string Description { get; set; } = null!;

    public static DirectorDto FromEntity(Director director) => new()
    {
        Id = director.Id,
        FullName = director.FullName,
        Gender = director.Male ? "Male" : "Female",
        Dob = director.Dob,
        DobString = director.Dob.ToString("M/d/yyyy"),
        Nationality = director.Nationality,
        Description = director.Description
    };
}
