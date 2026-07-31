using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public static class DesignerMapper
    {
        public static DesignerDto ToDesignerDto(this Designer designer)
        {
            return new DesignerDto
            {
                Id = designer.Id,
                FullName = designer.FullName,
                Gender = designer.Male ? "Male" : "Female",
                Dob = designer.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = designer.Dob.ToString(CultureInfo.InvariantCulture),
                Nationality = designer.Nationality,
                Description = designer.Description,
            };
        }

        public static DesignerWithProductDto ToDesignerWithProductDto(this Designer designer)
        {
            return new DesignerWithProductDto
            {
                Id = designer.Id,
                FullName = designer.FullName,
                Gender = designer.Male ? "Male" : "Female",
                Dob = designer.Dob.ToDateTime(TimeOnly.MinValue),
                DobString = designer.Dob.ToString(CultureInfo.InvariantCulture),
                Nationality = designer.Nationality,
                Description = designer.Description,
                Products = designer.Products?.Select(p => p.ToProductByDesignerDto()).ToList() ?? new List<ProductByDesignerDto>(),
            };
        }

        public static ProductByDesignerDto ToProductByDesignerDto(this Product product)
        {
            return new ProductByDesignerDto
            {
                Id = product.Id,
                Name = product.Name,
                LaunchDate = product.LaunchDate.HasValue ? product.LaunchDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                LaunchYear = product.LaunchDate.HasValue ? (int?)product.LaunchDate.Value.Year : null,
                Description = product.Description,
                Material = product.Material,
                ManufacturerId = product.ManufacturerId,
                DesignerId = product.DesignerId,
                ManufacturerName = product.Manufacturer != null ? product.Manufacturer.Name : null,
                DesignerName = product.Designer != null ? product.Designer.FullName : null,
            };
        }
    }
}
