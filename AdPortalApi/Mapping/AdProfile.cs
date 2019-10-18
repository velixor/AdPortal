using System;
using System.IO;
using AdPortalApi.Models;
using AdPortalApi.Services;
using AutoMapper;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Http;

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