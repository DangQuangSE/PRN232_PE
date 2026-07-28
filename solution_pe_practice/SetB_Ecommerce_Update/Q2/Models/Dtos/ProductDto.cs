namespace Q2.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string Material { get; set; } = null!;
        public DesignerDto? Designer { get; set; }
        public List<ReviewerDto> Reviewers { get; set; } = [];
    }
    public class DesignerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
    }
    public class ReviewerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
    }
}
