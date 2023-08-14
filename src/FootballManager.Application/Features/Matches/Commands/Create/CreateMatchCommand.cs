using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Create
{
    public record CreateMatchCommand : RequestAudit, IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Teamsize { get; set; }
        public int TeamCount { get; set; }
    }
}
