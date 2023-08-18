using AutoMapper;
using AutoMapper.QueryableExtensions;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.ResultModels;
using FootballManager.Infrastructure.Extensions;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetAll
{
    public record GetAllMatchQuery : IRequest<PaginatedResult<GetAllMatchDto>>;

    internal class GetAllMatchQueryHandler : IRequestHandler<GetAllMatchQuery, PaginatedResult<GetAllMatchDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Match> _matchRepository;

        public GetAllMatchQueryHandler(IMapper mapper, IAsyncRepository<Match> matchRepository)
        {
            _mapper = mapper;
            _matchRepository = matchRepository;
        }

        public async Task<PaginatedResult<GetAllMatchDto>> Handle(GetAllMatchQuery request, CancellationToken cancellationToken)
        {
            var data = _matchRepository.Entities.Where(x => !x.IsDeleted);

            return await data.ProjectTo<GetAllMatchDto>(_mapper.ConfigurationProvider)
                             .ToPaginatedListAsync(1, data.Count(), cancellationToken);
        }
    }
}
