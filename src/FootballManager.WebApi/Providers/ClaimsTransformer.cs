using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FootballManager.Application.Extensions;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.OptionsSettings;
using Microsoft.AspNetCore.Authentication;

namespace FootballManager.WebApi.Providers
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly JwtOptions _jwtOptions;

        public ClaimsTransformer(IAsyncRepository<User> userRepository,
            JwtOptions jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                if (principal.Identity.IsAuthenticated)
                {
                    var username = principal.GetUsername();
                    var user =  _userRepository.Entities.Where(x=>x.Username.Equals(username)).FirstOrDefault();

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

                    principal.AddIdentity(new ClaimsIdentity(claims));
                }

                return principal;
            }
            catch (Exception)
            {
                return principal;
            }
        }
    }
}
