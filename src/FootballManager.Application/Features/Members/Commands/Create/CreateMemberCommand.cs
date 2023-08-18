using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Commands.Create
{
    public record CreateMemberCommand : RequestAudit, IRequest<Result<int>>
    {
        public string Name { get; set; }
        public int Elo { get; set; }
        public string Description { get; set; }
        public int? PositionId { get; set; }
    }
}
