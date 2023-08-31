using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchScores.Commands.Update
{
    internal class UpdateMatchScoreCommandHandler : IRequestHandler<UpdateMatchScoreCommand, Result<bool>>
    {
        private readonly IAsyncRepository<MatchScore> _matchScoreRepository;

        public UpdateMatchScoreCommandHandler(IAsyncRepository<MatchScore> matchScoreRepository)
        {
            _matchScoreRepository = matchScoreRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMatchScoreCommand request, CancellationToken cancellationToken)
        {
            var matchScore = await _matchScoreRepository.GetAsync(request.Id) ?? throw new DomainException("Match score not found");

            matchScore.NumberGoalTeam1 = (short)request.NumberGoalTeam1;
            matchScore.NumberGoalTeam2 = (short)request.NumberGoalTeam2;
            matchScore.ModifiedBy = request.RequestedBy;
            matchScore.ModifiedDate = request.RequestedAt;

            var updated = await _matchScoreRepository.UpdateAsync(matchScore);

            return updated != null ? Result<bool>.Success(true) : Result<bool>.Failure();
        }
    }
}
