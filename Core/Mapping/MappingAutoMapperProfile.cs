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
            CreateMap<Ad, AdResponse>();
            CreateMap<AdRequest, Ad>();

            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>()
                .ForMember(response => response.AdsCount,
                    opt => opt
                        .MapFrom(user => user.Ads.Count));
        }
    }
}