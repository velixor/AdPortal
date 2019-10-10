using AdPortalApi.Models;
using AutoMapper;
using Dtos.Contracts.Requests;

namespace AdPortalApi.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<AdRequest, Ad>();
        }
    }
}