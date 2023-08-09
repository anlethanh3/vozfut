using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.DataAccess.Repositories;
using FootballManager.Logic.Business.Interfaces;
using FootballManager.Logic.Business.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// Health check
builder.Services.AddHealthChecks();
// Dependency Injection
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddDbContext<EntityDbContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
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

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// health check
app.MapHealthChecks("/healthz");

app.Run();
