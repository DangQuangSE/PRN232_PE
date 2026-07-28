namespace givenAPI.Models
{
    public partial class Reviewer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool? Male { get; set; }
        public DateTime? Dob { get; set; }
        public string? Description { get; set; }
        public string? Nationality { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
