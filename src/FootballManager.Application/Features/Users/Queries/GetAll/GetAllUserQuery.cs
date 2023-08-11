using AutoMapper;
using AutoMapper.QueryableExtensions;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.ResultModels;
using FootballManager.Infrastructure.Extensions;
using MediatR;

namespace FootballManager.Application.Features.Users.Queries.GetAll
{
    public record GetAllUserQuery(int Page, int Limit) : IRequest<PaginatedResult<GetAllUserDto>>;

    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PaginatedResult<GetAllUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(IUserRepository userRepository,
             IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllUserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.Entities.Where(x => !x.IsDeleted)
                                        .ProjectTo<GetAllUserDto>(_mapper.ConfigurationProvider)
                                        .ToPaginatedListAsync(request.Page, request.Limit, cancellationToken);
        }
    }
}
