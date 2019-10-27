using AutoMapper;
using Data.Models;
using Dto.Contracts.AdContracts;
using Dto.Contracts.UserContracts;

namespace Core.Mapping
{
    public class MappingAutoMapperProfile : Profile
    {
        public MappingAutoMapperProfile()
        {
            CreateMap<Ad, AdResponse>().ForMember(response => response.Image,
                opt => opt.MapFrom(ad => ad.ImageName));
            CreateMap<AdUpdateRequest, Ad>();
            CreateMap<AdCreateRequest, Ad>();

            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}