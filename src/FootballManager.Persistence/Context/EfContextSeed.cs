using FootballManager.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FootballManager.Persistence.Context
{
    public class EfContextSeed
    {
        public static async Task SeedAsync(EfDbContext context, ILogger<EfContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(GetPreConfigureOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associted with context {DbContext}", typeof(EfDbContext).Name);
            }
        }

        private static List<User> GetPreConfigureOrders()
        {
            return new List<User> {
                new User
                {
                     //Id = 1,
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
        }
    }
}
