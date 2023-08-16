using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
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
                TeamSize = request.TeamSize,
                MatchDate = request.MatchDate,
                TotalAmount = request.TotalAmount,
                TotalHour = request.TotalHour,
                FootballFieldAddress = request.FootballFieldAddress,
                FootballFieldNumber = request.FootballFieldNumber,
                FootballFieldSize = request.FootballFieldSize,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = MatchStatusEnum.ComingSoon.Name,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var result = await _matchRepository.CreateAsync(match);

            return result != null
                          ? Result<int>.Success(1)
                          : Result<int>.Failure();
        }
    }
}
