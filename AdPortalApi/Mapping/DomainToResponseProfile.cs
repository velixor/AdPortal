using AdPortalApi.Models;
using AutoMapper;
using Dto.Contracts.AdContracts;
using Dto.Contracts.UserContracts;

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