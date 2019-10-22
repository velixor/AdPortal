using AutoMapper;
using Data.Models;
using Dto.Contracts.AdContracts;
using Dto.Contracts.UserContracts;

namespace Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
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