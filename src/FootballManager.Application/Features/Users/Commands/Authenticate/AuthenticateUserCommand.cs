using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Commands.Authenticate
{
    public record AuthenticateUserCommand(string UserName, string Password, string Email):IRequest<Result<AuthenticateUserResponse>>;
}
