using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Persistence.Context;
using FootballManager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddGenericRepository()
                           .AddConnectionAndDbContext(configuration);
        }

        private static IServiceCollection AddGenericRepository(this IServiceCollection services)
        {
            return services
                           .AddScoped<IUserRepository, UserRepository>();
        }

        private static IServiceCollection AddConnectionAndDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("SqlConnectionString");
            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            return services;
        }
    }
}
