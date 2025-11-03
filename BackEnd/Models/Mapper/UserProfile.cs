using AutoMapper;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;

namespace itsc_dotnet_practice.Models.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"));
        }
    }
}