using System.Security.Claims;
using IdentityModel;

namespace FootballManager.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get current Id
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(JwtClaimTypes.Id)?.Value;
        }

        /// <summary>
        /// Get current Username
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUsername(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(JwtClaimTypes.PreferredUserName)?.Value;
        }
    }
}
