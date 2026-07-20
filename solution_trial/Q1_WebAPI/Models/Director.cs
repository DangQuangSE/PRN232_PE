namespace Q1_WebAPI.Models;

public class Director
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public bool Male { get; set; }
    public DateTime Dob { get; set; }
    public string Nationality { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
