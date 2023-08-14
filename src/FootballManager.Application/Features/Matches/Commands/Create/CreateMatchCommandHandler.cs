using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Create
{
    internal class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<int>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;

        public CreateMatchCommandHandler(IAsyncRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Result<int>> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = new Match
            {
                Name = request.Name,
                Description = request.Description,
                TeamCount = request.TeamCount,
                TeamSize = request.Teamsize,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var result = await _matchRepository.CreateAsync(match);

            if (result != null)
            {
                return await Result<int>.SuccessAsync(1);
            }
            else
            {
                return Result<int>.Failure();
            }
        }
    }
}
