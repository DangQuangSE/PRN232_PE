namespace Q1.Models.Dtos
{
    public class ProductByDesignerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? LaunchDate { get; set; }
        public int? LaunchYear { get; set; }

        public string Description { get; set; }

        public string Material { get; set; }

        public int? ManufacturerId { get; set; }

        public int? DesignerId { get; set; }
        public string? ManufacturerName { get; set; }
        public string? DesignerName { get; set; }
        public List<Tag>? Tags { get; set; } = [];
        public List<Reviewer> Reviewers { get; set; } = [];

    }
}
