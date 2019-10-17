﻿﻿using AdPortalApi.Models;
using AutoMapper;
 using Dto.Contracts.UserContracts;

 namespace AdPortalApi.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}