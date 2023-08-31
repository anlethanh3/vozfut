using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchScores.Commands.Update
{
    public record UpdateMatchScoreCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public int NumberGoalTeam1 { get; set; }
        public int NumberGoalTeam2 { get; set; }
    }
}
