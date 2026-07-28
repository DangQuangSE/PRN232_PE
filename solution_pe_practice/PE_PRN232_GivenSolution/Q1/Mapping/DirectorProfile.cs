using AutoMapper;
using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Mapping
{
    public class DirectorProfile : Profile
    {
        public DirectorProfile()
        {
            CreateMap<Director, DirectorDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Male ? "Male" : "Female"))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.DobString, opt => opt.MapFrom(src => src.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Director, DirectorWithMoviesDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Male ? "Male" : "Female"))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.DobString, opt => opt.MapFrom(src => src.Dob.ToString("M/d/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.ReleaseDate,
                    opt => opt.MapFrom(src => src.ReleaseDate.HasValue ? src.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(dest => dest.ReleaseYear,
                    opt => opt.MapFrom(src => src.ReleaseDate.HasValue ? (int?)src.ReleaseDate.Value.Year : (null)))
                .ForMember(dest => dest.ProducerName, otp => otp.MapFrom(src => src.Producer != null ? src.Producer.Name : null))
                .ForMember(dest => dest.DirectorName, otp => otp.MapFrom(src => src.Director != null ? src.Director.FullName : null))
                .ForMember(dest => dest.Genres, otp => otp.Ignore())
                .ForMember(dest => dest.Stars, otp => otp.Ignore());
        }
    }
}
