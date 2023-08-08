using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services)
        {
            services.AddGenericRepository();

            return services;
        }

        private static IServiceCollection AddGenericRepository(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IAsyncRepository<,,>), typeof(EfBaseRepository<,,>));
        }
    }
}
