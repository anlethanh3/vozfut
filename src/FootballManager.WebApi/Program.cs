using FootballManager.Application.Extensions;
using FootballManager.Persistence.Extensions;
using FootballManager.WebApi.Extensions;
using FootballManager.WebApi.Helpers;
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

app.UsePresentationLayer(configuration)
   .UseMinimalApi();

await app.RunAsync();
