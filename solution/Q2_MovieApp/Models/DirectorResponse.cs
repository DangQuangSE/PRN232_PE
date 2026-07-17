namespace Q2_MovieApp.Models;

public class DirectorResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public bool Male { get; set; }
    public DateTime Dob { get; set; }
    public string Nationality { get; set; } = null!;
    public string Description { get; set; } = null!;
}
