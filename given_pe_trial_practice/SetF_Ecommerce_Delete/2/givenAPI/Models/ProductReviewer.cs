namespace givenAPI.Models
{
    public partial class ProductReviewer
    {
        public int ProductId { get; set; }
        public int ReviewerId { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Reviewer Reviewer { get; set; } = null!;
    }
}
