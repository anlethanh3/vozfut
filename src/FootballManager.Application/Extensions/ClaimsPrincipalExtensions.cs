using System.Security.Claims;

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

            return principal.FindFirst("UserId")?.Value;
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

            return principal.FindFirst("Username")?.Value;
        }

        /// <summary>
        /// Get current Email
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("Email")?.Value;
        }

        /// <summary>
        /// Get current Name
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("Name")?.Value;
        }

        /// <summary>
        /// Get current Name
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetRole(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
