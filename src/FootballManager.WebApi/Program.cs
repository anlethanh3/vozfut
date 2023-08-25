using FootballManager.Application.Extensions;
using FootballManager.Infrastructure.Helpers;
using FootballManager.Persistence.Extensions;
using FootballManager.WebApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = ConfigurationHelper.GetConfiguration();

var services = builder.Services;

builder.Host.UseSerilog(LoggingExtension.ConfigureLogging());

#region dependency injection layer

services.AddPresentationLayer(configuration)
        .AddApplicationLayer(configuration)
        .AddPersistenceLayer(configuration);

#endregion dependency injection layer

var app = builder.Build();
var environment = app.Environment;

app.UsePresentationLayer(configuration, environment)
   .UseMinimalApi();

await app.RunAsync();
