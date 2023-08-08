using AutoMapper;

namespace FootballManager.Application.AutoMappings
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            //    CreateMap<User, CreateUserCommand>().ReverseMap();
            //    CreateMap<UserProfile, CreateUserCommand>().ReverseMap();
            //    CreateMap<CreateUserAddressCommand, UserAddress>()
            //        .ForMember(dest => dest.DisplayName,
            //                   opt => opt.MapFrom(src => src.FullName))
            //        .ReverseMap();

            //    CreateMap<UpdateUserCommand, UserProfile>().ReverseMap();
        }
    }
}
