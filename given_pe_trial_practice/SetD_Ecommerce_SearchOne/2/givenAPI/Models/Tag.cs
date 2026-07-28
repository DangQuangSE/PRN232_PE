namespace givenAPI.Models
{
    public partial class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
