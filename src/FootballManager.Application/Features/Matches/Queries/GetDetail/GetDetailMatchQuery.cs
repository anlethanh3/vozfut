using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public record GetDetailMatchQuery(int Id) : IRequest<Result<GetDetailMatchDto>>;

    internal class GetDetailMatchQueryHandler : IRequestHandler<GetDetailMatchQuery, Result<GetDetailMatchDto>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;

        public GetDetailMatchQueryHandler(IAsyncRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Result<GetDetailMatchDto>> Handle(GetDetailMatchQuery request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetAsync(request.Id) ?? throw new DomainException("Match not found");

            return await Result<GetDetailMatchDto>.SuccessAsync(new GetDetailMatchDto
            {
                Id = match.Id,
                Name = match.Name,
                Description = match.Description,
                TeamCount = match.TeamCount,
                TeamSize = match.TeamSize,
                CreatedBy = match.CreatedBy,
                CreatedDate = match.CreatedDate
            });
        }
    }
}
