namespace Q1.Models.Dtos
{
    public class DesignerWithProductDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime Dob { get; set; }
        public string DobString { get; set; }

        public string Nationality { get; set; }

        public string Description { get; set; }
        public List<ProductByDesignerDto>? Products { get; set; }    
    }
}
