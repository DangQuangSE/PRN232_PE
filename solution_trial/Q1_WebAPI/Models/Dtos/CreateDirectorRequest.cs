namespace Q1_WebAPI.Models.Dtos;

public class CreateDirectorRequest
{
    public string FullName { get; set; } = null!;
    public bool Male { get; set; }
    public DateTime Dob { get; set; }
    public string Nationality { get; set; } = null!;
    public string Description { get; set; } = null!;
}
