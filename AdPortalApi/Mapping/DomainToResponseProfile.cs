using AdPortalApi.Contracts.Responses;
using AdPortalApi.Models;
using AutoMapper;

namespace AdPortalApi.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Ad, AdResponse>()
                .ForMember(adRes => adRes.UserId, opt =>
                {
                    opt.MapFrom(src => src.User.Id);
                });
        }
    }
}