using AutoMapper;
using FootballManager.Application.Features.Users.Queries.GetAll;
using FootballManager.Domain.Entities;

namespace FootballManager.Application.AutoMappings
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<User, GetAllUserDto>().ReverseMap();
        }
    }
}
