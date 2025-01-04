using AutoMapper;
using Library.Management.System.API.DTOs;
using Library.Management.System.API.Models;

namespace Library.Management.System.API.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDTO, Book>().ReverseMap();

            CreateMap<BookUpdateDTO, Book>();

            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
