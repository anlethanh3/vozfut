using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.WebApi.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrationDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            // TODO: should replace retry policy
            var retryAvailability = retry ?? 0;
            using var scope = host.Services.CreateScope();
            var service = scope.ServiceProvider;
            var logger = service.GetRequiredService<ILogger<TContext>>();
            var context = service.GetRequiredService<TContext>();

            try
            {
                //var sw = new Stopwatch();
                logger.LogInformation("Migrating database from context {DbContextName} and start at time utc {StartUTC}",
                                       typeof(TContext).Name, DateTime.UtcNow);

                InvokeSeeder(seeder, context, service);

                logger.LogInformation("Migrated database from context {DbContextName} and end at time utc {EndUTC}",
                                        typeof(TContext).Name, DateTime.UtcNow);
            }
            catch (SqlException ex)
            {
                logger.LogError("An error when run migration with exception {ExceptionMessage}", ex.Message);

                if (retryAvailability < 50)
                {
                    retryAvailability++;
                    Thread.Sleep(1500);
                    MigrationDatabase<TContext>(host, seeder, retryAvailability);
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                                   TContext context,
                                                   IServiceProvider service)
                                                   where TContext : DbContext
        {
            context.Database.EnsureCreated();
            seeder(context, service);
        }
    }
}
