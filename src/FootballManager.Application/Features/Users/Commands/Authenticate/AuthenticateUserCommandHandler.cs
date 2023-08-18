using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace FootballManager.Application.Features.Users.Commands.Authenticate
{
    internal class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<AuthenticateUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;

        public AuthenticateUserCommandHandler(IUserRepository userRepository, JwtOptions jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions;
        }

        public async Task<Result<AuthenticateUserResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.Entities.Where(x => x.Email.Equals(request.Email) && !x.IsDeleted)?.FirstOrDefault() ?? throw new DomainException("ERR_USER_NOT_EXIST");

            var isAuthorize = await _userRepository.ValidatePasswordAsync(user, request.Password);
            if (!isAuthorize)
            {
                throw new DomainException("ERR_UNAUTHORIZED");
            }

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub,_jwtOptions.Subject??string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("UserId",$"{user.Id}"),
                new Claim("Name",$"{user.Name}"),
                new Claim("Email",$"{user.Email}"),
                new Claim("Username",$"{user.Username}"),
                new Claim(ClaimTypes.Role, user.IsAdmin?"Admin":"User")
            };

            // generate token that is valid for 1 hour
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key ?? string.Empty);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new AuthenticateUserResponse(tokenHandler.WriteToken(token), "Bearer", 3600);

            return await Result<AuthenticateUserResponse>.SuccessAsync(result);
        }
    }
}
