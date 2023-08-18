using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Create
{
    public record CreateMatchDetailCommand : RequestAudit, IRequest<Result<int>>
    {
        public int MatchId { get; set; }
        public int MemberId { get; set; }
        public string BibColour { get; set; }
    }
}
