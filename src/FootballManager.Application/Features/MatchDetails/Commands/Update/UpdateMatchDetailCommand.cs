using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Commands.Update
{
    public record UpdateMatchDetailCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int MemberId { get; set; }
        public string BibColour { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSkip { get; set; }
    }
}
