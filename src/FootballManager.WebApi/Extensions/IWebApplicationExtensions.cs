using FootballManager.Persistence.Context;
using FootballManager.WebApi.Providers;

namespace FootballManager.WebApi.Extensions
{
    public static class IWebApplicationExtensions
    {
        public static WebApplication UsePresentationLayer(this WebApplication app, IConfiguration configuration)
        {
            return app.UseCustomSwagger()
                      .UseCustomMiddleware()
                      .UseCustomCors()
                      .UseCustomIdentityServer(configuration)
                      .UseCustomMigrate();
        }

        private static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            return app;
        }

        private static WebApplication UseCustomSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                // Build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
            return app;
        }

        private static WebApplication UseCustomCors(this WebApplication app)
        {
            app.UseRouting()
               .UseCors("AllowAllOrigins")
               .UseHttpsRedirection()
               .UseRouting();
            return app;
        }

        private static WebApplication UseCustomIdentityServer(this WebApplication app, IConfiguration configuration)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        private static WebApplication UseCustomMigrate(this WebApplication app)
        {
            app.CreateDbIfNotExists<EfDbContext>((context, service) =>
            {
                var logger = service.GetService<ILogger<EfContextSeed>>();
                EfContextSeed.SeedAsync(context, logger).Wait();
            });

            return app;
        }
    }
}
