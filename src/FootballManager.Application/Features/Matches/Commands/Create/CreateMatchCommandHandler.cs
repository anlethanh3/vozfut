using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using FootballManager.Infrastructure.Helpers;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Create
{
    internal class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<int>>
    {
        private readonly IAsyncRepository<Match> _matchRepository;
        private readonly IAsyncRepository<Vote> _votecRepository;

        public CreateMatchCommandHandler(IAsyncRepository<Match> matchRepository,
            IAsyncRepository<Vote> voteRepository)
        {
            _matchRepository = matchRepository;
            _votecRepository = voteRepository;
        }

        public async Task<Result<int>> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = new Match
            {
                Name = request.Name,
                Code = RandomHelper.RandomString(6),
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

            if (request.VoteId.HasValue)
            {
                _ = _votecRepository.Entities.Where(x => x.Id == request.VoteId.Value && !x.IsDeleted) ?? throw new DomainException("Vote not found");
                match.VoteId = request.VoteId.Value;
            }

            var result = await _matchRepository.CreateAsync(match);

            return result != null
                          ? Result<int>.Success(1)
                          : Result<int>.Failure();
        }
    }
}
