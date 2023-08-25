using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Commands.Update
{
    public record UpdateMemberCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Elo { get; set; }
        public int? PositionId { get; set; }
        public int? SubPositionId { get; set; }
    }
}
