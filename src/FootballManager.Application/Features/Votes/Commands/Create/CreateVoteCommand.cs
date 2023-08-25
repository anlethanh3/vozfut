using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Commands.Create
{
    public record CreateVoteCommand : RequestAudit, IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
