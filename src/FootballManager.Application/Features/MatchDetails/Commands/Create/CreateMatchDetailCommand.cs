using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Create
{
    public record CreateMatchDetailCommand : RequestAudit, IRequest<Result<int>>
    {
        public List<CreateTeamMatchDetailCommand> Teams { get; set; }
    }

    public class CreateTeamMatchDetailCommand
    {
        public int MatchId { get; set; }
        public int[] MemberId { get; set; }
        public string BibColour { get; set; }
    }

    public class TeamDto
    {
        public string TeamA { get; set; }
        public string TeamB { get; set; }
    }
}
