using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Repositories
{
    public class UserRepository : EfBaseRepository<User>, IUserRepository
    {
        public UserRepository(EfDbContext genericContext) : base(genericContext)
        {
        }

        public async Task<bool> CheckExistUserNameAsync(string username)
        {
            var query = await GenericContext.Users.FirstOrDefaultAsync(x => x.Username.Equals(username));
            return query != null;
        }

        public async Task<string> GeneratePasswordHashAsync(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, $"{password}");
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, $"{password}");
            return result != PasswordVerificationResult.Failed;
        }
    }
}
