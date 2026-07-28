using AutoMapper;
using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public class DesignerProfile : Profile
    {
        public DesignerProfile()
        {
            CreateMap<Designer, DesignerDto>()
               .ForMember(dest => dest.Dob, otp => otp.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
               .ForMember(dest => dest.Gender, otp => otp.MapFrom(src => src.Male ? "Male" : "Female"))
               .ForMember(dest => dest.DobString, otp => otp.MapFrom(src => src.Dob.ToString(CultureInfo.InvariantCulture)));
            CreateMap<Designer, DesignerWithProductDto>()
               .ForMember(dest => dest.Dob, otp => otp.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
               .ForMember(dest => dest.Gender, otp => otp.MapFrom(src => src.Male ? "Male" : "Female"))
               .ForMember(dest => dest.DobString, otp => otp.MapFrom(src => src.Dob.ToString(CultureInfo.InvariantCulture)));
            CreateMap<Product, ProductByDesignerDto>()
                .ForMember(dest => dest.LaunchDate,
                otp => otp.MapFrom(src => src.LaunchDate.HasValue ? src.LaunchDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(dest => dest.LaunchYear,
                otp => otp.MapFrom(src => src.LaunchDate.HasValue ? (int?)src.LaunchDate.Value.Year : (null)))
                .ForMember(dest => dest.ManufacturerName,
                otp => otp.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.Name : null))
                .ForMember(dest => dest.DesignerName,
                otp => otp.MapFrom(src => src.Designer != null ? src.Designer.FullName : null));
        }

    }
}
