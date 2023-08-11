using System.Security.Claims;
using FootballManager.Application.Extensions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Queries.Profile
{
    public record GetProfileUserDto(int UserId, string UserName, string Email, string Role);
    public record GetProfileUserQuery : IRequest<Result<GetProfileUserDto>>;

    internal class GetProfileUserQueryHandler : IRequestHandler<GetProfileUserQuery, Result<GetProfileUserDto>>
    {
        private readonly ClaimsPrincipal _principal;

        public GetProfileUserQueryHandler(ClaimsPrincipal principal)
        {
            _principal = principal;
        }

        public async Task<Result<GetProfileUserDto>> Handle(GetProfileUserQuery request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_principal.GetUserId() ?? "0");
            var email = _principal.GetEmail() ?? string.Empty;
            var username = _principal.GetUsername() ?? string.Empty;
            var role = _principal.GetRole() ?? "User";
            var profile = new GetProfileUserDto(userId, username, email, role);

            return await Result<GetProfileUserDto>.SuccessAsync(profile);
        }
    }
}
