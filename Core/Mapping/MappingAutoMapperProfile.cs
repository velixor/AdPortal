using AutoMapper;
using Data.Models;
using Dto.Contracts.AdContracts;
using Dto.Contracts.UserContracts;

namespace Core.Mapping
{
    public class MappingAutoMapperProfile : Profile
    {
        public MappingAutoMapperProfile()
        {
            CreateMap<Ad, AdResponse>().ForMember(response => response.Image,
                opt => opt.MapFrom(ad => ad.ImageName));
            CreateMap<AdUpdateRequest, Ad>();
            CreateMap<AdCreateRequest, Ad>();

            CreateMap<UserRegisterRequest, User>();
            CreateMap<UserRegisterRequest, UserLoginRequest>();
            CreateMap<User, UserResponse>();
            CreateMap<User, UserLoggedResponse>();
            CreateMap<UserEdit, User>().ForMember(user => user.Password,
                    opt => opt.MapFrom(edit => edit.NewPassword))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<User, UserEdit>();
        }
    }
}