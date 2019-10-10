using AdPortalApi.Models;
using AutoMapper;
using Dtos.Contracts.Responses;

namespace AdPortalApi.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Ad, AdResponse>();
        }
    }
}