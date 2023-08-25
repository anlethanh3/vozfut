using System.Reflection;
using System.Security.Claims;
using FluentValidation;
using FootballManager.Application.AutoMappings;
using FootballManager.Application.Behaviours;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.BindConfigOptions(configuration)
                    .AddMediator()
                    .AddValidators()
                    .AddCustomAutoMapper()
                    .AddcustomPrincipal();

            return services;
        }

        private static IServiceCollection BindConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOption = configuration.GetOptions<SwaggerOptions>("Swagger");
            services.AddSingleton<SwaggerOptions>(swaggerOption);

            var connectionOptions = configuration.GetOptions<ConnectionOptions>("ConnectionStrings");
            services.AddSingleton<ConnectionOptions>(connectionOptions);

            var jwtOptions = configuration.GetOptions<JwtOptions>("Jwt");
            services.AddSingleton<JwtOptions>(jwtOptions);

            var bibColour = configuration.GetSection("BibColour").Get<List<string>>();
            services.AddSingleton(new BibColourOption { BibColour = bibColour });

            return services;
        }

        private static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(AutoMap));
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(AuditBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
            });

            return services;
        }

        private static IServiceCollection AddcustomPrincipal(this IServiceCollection services)
        {
            return services.AddTransient<ClaimsPrincipal>(x =>
            {
                IHttpContextAccessor currentContext = x.GetService<IHttpContextAccessor>();

                if (currentContext?.HttpContext != null)
                {
                    return currentContext.HttpContext.User;
                }

                return new ClaimsPrincipal();
            });
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
