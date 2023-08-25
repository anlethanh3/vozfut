using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Commands.Update
{
    public record UpdateVoteCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
