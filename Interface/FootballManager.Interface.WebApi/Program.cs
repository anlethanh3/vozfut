using System.Reflection;
using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.DataAccess.Repositories;
using FootballManager.Logic.Business.Interfaces;
using FootballManager.Logic.Business.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Health check
builder.Services.AddHealthChecks();
// Dependency Injection
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IMatchRepository, MatchRepository>();
builder.Services.AddTransient<IMatchDetailRepository, MatchDetailRepository>();
builder.Services.AddTransient<IDatabaseContext, DapperDbContext>();
builder.Services.AddDbContext<EntityDbContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.SwaggerDoc("v1", new()
    {
        Version = "v1",
        Title = "VozFut WebApi",
        Description = "VozFut WebApi for managing VozFut club",
        TermsOfService = new("https://github.com/anlethanh3/vozfut/TERM"),
        Contact = new()
        {
            Name = "Contact",
            Url = new("https://github.com/anlethanh3/vozfut"),
        },
        License = new()
        {
            Name = "License",
            Url = new("https://github.com/anlethanh3/vozfut/blob/main/LICENSE"),
        },

    });
    options.AddSecurityDefinition("Bearer", new()
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

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
