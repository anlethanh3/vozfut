using FootballManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FootballManager.Persistence.Context
{
    public class EfContextSeed
    {
        public static async Task SeedAsync(EfDbContext context, ILogger<EfContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(GetPreConfigureUsers());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed table {Table} database associted with context {Dbcontext}", "Users", typeof(EfContextSeed).Name);
            }

            if (!context.Members.Any())
            {
                context.Members.AddRange(GetPreConfigureMembers());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed table {Table} database associted with context {DbContext}", "Members", typeof(EfDbContext).Name);
            }
        }

        private static List<User> GetPreConfigureUsers()
            => new()
            {
                new User
                {
                    Username = "tamn.chichi",
                    PasswordHash = GeneratePasswordHash(null, "Tamn0310@"),
                    Email = "tamn0310@gmail.com",
                    Name = "Tâm",
                    IsAdmin = true,
                    MemberId = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new User
                {
                    Username = "anle",
                    PasswordHash = GeneratePasswordHash(null, "12345678"),
                    Email = "an.lethanh3@gmail.com",
                    Name = "AnLe",
                    IsAdmin = true,
                    MemberId = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

        private static List<Member> GetPreConfigureMembers()
            => new()
            {
                new Member
                {
                     Name = "Nguyễn Tâm",
                     Elo = 3,
                     CreatedBy = "System",
                     CreatedDate = DateTime.UtcNow,
                     DeletedDate = null,
                     IsDeleted = false,
                     Description = "Description",
                     ModifiedBy = null,
                     ModifiedDate = null
                }
            };

        private static string GeneratePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var hash = passwordHasher.HashPassword(user, $"{password}");
            return hash;
        }
    }
}
