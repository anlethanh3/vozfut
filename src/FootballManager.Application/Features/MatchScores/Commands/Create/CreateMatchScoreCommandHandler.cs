using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Features.MatchScores.Commands.Create
{
    internal class CreateMatchScoreCommandHandler : IRequestHandler<CreateMatchScoreCommand, Result<int>>
    {
        private readonly IAsyncRepository<MatchScore> _matchScoreRepository;
        private readonly IAsyncRepository<Match> _matchRepository;

        public CreateMatchScoreCommandHandler(IAsyncRepository<MatchScore> matchScoreRepository,
            IAsyncRepository<Match> matchRepository)
        {
            _matchScoreRepository = matchScoreRepository;
            _matchRepository = matchRepository;
        }

        public async Task<Result<int>> Handle(CreateMatchScoreCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.Entities.Where(x => x.Id == request.MatchId && !x.IsDeleted)
                                                       .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                        ?? throw new DomainException("Match not foundm ");

            // Thi đấu theo vòng tròn tính điểm
            var checkTeamExistMatchScore = _matchScoreRepository.Entities.Where(x => x.MatchId == request.MatchId && x.Team1.Equals(request.Team1)).ToList();
            if (checkTeamExistMatchScore.Count > 1)
            {
                throw new DomainException("Team already exists in match");
            }

            var matchScore = new MatchScore
            {
                MatchId = request.MatchId,
                Team1 = request.Team1,
                Team2 = request.Team2,
                NumberGoalTeam1 = request.NumberGoalTeam1,
                NumberGoalTeam2 = request.NumberGoalTeam2,
                Type = request.Type,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var created = await _matchScoreRepository.CreateAsync(matchScore);

            return created != null ? Result<int>.Success(1) : Result<int>.Failure();
        }
    }
}
