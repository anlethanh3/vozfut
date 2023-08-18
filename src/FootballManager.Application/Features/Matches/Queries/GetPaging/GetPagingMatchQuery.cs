using AutoMapper;
using AutoMapper.QueryableExtensions;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.ResultModels;
using FootballManager.Infrastructure.Extensions;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetPaging
{
    public record GetPagingMatchQuery(int Page, int Limit) : IRequest<PaginatedResult<GetPagingMatchDto>>;

    internal class GetPagingMatchQueryHandler : IRequestHandler<GetPagingMatchQuery, PaginatedResult<GetPagingMatchDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Match> _matchRepository;

        public GetPagingMatchQueryHandler(IMapper mapper, IAsyncRepository<Match> matchRepository)
        {
            _mapper = mapper;
            _matchRepository = matchRepository;
        }

        public async Task<PaginatedResult<GetPagingMatchDto>> Handle(GetPagingMatchQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.Entities.Where(x => !x.IsDeleted)
                                         .ProjectTo<GetPagingMatchDto>(_mapper.ConfigurationProvider)
                                         .ToPaginatedListAsync(request.Page, request.Limit, cancellationToken);
        }
    }
}
