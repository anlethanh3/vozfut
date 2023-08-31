using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchScores.Commands.Create
{
    public record CreateMatchScoreCommand() : RequestAudit, IRequest<Result<int>>
    {
        public int MatchId { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public short NumberGoalTeam1 { get; set; }
        public short NumberGoalTeam2 { get; set; }
        public string Type { get; set; }
    }
}
