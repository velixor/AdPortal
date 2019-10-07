using AdPortalApi.Contracts.Requests;
using AdPortalApi.Models;
using AutoMapper;

namespace AdPortalApi.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<AdRequest, Ad>()
                .ForMember(ad => ad.User, opt =>
                {
                    opt.MapFrom(src => new User {Id = src.UserId});
                });
        }
    }
}