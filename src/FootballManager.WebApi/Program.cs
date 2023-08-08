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
        .AddInfrastructure(configuration);

#endregion dependency injection layer
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
