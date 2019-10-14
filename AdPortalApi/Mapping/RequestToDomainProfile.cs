﻿using AdPortalApi.Models;
using AutoMapper;
 using Dto.Contracts.AdContracts;
 using Dto.Contracts.UserContracts;

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