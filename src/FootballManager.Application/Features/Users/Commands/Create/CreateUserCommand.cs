using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Commands.Create
{
    public record CreateUserCommand : RequestAudit, IRequest<Result<int>>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public int? MemberId { get; set; }
    }
}
