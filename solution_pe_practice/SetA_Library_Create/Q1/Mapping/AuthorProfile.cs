using AutoMapper;
using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Gender, otp => otp.MapFrom(src => src.Male ? "Male" : "Female"))
                .ForMember(dest => dest.Dob, otp => otp.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.DobString, otp => otp.MapFrom(src => src.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Author, AuthorWithBookDTO>()
                .ForMember(dest => dest.Gender, otp => otp.MapFrom(src => src.Male ? "Male" : "Female"))
                .ForMember(dest => dest.Dob, otp => otp.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.DobString, otp => otp.MapFrom(src => src.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Book, BookByAuthor>()
                .ForMember(dest => dest.PublishDate,
                otp => otp.MapFrom(src => src.PublishDate.HasValue ? src.PublishDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(dest => dest.PublishYear,
                otp => otp.MapFrom(src => src.PublishDate.HasValue ? (int?)src.PublishDate.Value.Year : (null)))
                .ForMember(dest => dest.PublisherName,
                otp => otp.MapFrom(src => src.Publisher != null ? src.Publisher.Name : null))
                .ForMember(dest => dest.AuthorName, otp => otp.MapFrom(src => src.Author != null ? src.Author.FullName : null))
                .ForMember(dest => dest.Geners, otp => otp.Ignore())
                .ForMember(dest => dest.Translators, otp => otp.Ignore());
        }
    }
}
