using AutoMapper;
using FootballManager.Application.Features.Matches.Queries.GetAll;
using FootballManager.Application.Features.Matches.Queries.GetPaging;
using FootballManager.Application.Features.Users.Queries.GetAll;
using FootballManager.Domain.Entities;

namespace FootballManager.Application.AutoMappings
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<User, GetAllUserDto>().ReverseMap();
            CreateMap<Match, GetAllMatchDto>().ReverseMap();
            CreateMap<Match, GetPagingMatchDto>().ReverseMap();
        }
    }
}
