namespace Q2.Models.Dtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string Material { get; set; } = null!;
        public int? DesignerId { get; set; }
    }
    public class UpdateProductRequest
    {
        public string Name { get; set; } = "";
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string Material { get; set; } = "";
        public int? DesignerId { get; set; }
    }
}
