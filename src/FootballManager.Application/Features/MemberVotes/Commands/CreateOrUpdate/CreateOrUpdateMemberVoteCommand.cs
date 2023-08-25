using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MemberVotes.Commands.Create
{
    public record CreateOrUpdateMemberVoteCommand : RequestAudit, IRequest<Result<int>>
    {
        public int MemberId { get; set; }
        public int VoteId { get; set; }
        public bool IsJoin { get; set; }
    }
}
