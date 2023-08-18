using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Persistence.Context;
using FootballManager.Persistence.DapperConnectionFactories;
using FootballManager.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FootballManager.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddGenericRepository()
                           .AddConnectionAndDbContext(configuration)
                           .AddDapperSqlFactory();
        }

        private static IServiceCollection AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfBaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
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

        private static IServiceCollection AddDapperSqlFactory(this IServiceCollection services)
        {
            services.AddTransient<SqlConnectionFactory>();

            services.AddTransient<ISqlConnectionFactory>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<SqlConnectionFactory>>();
                return new SqlConnectionFactory(logger);
            });

            return services;
        }
    }
}
