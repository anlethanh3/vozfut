using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Update
{
    public record UpdateMatchCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeamSize { get; set; }
        public int TeamCount { get; set; }
    }
}
