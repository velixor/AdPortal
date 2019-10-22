using AdPortalApi.Models;
using AutoMapper;
using Dto.Contracts.AdContracts;

namespace AdPortalApi.Mapping
{
    public class AdProfile : Profile
    {
        public AdProfile()
        {
            CreateMap<Ad, AdResponse>();
            CreateMap<AdRequest, Ad>();
        }
    }
}