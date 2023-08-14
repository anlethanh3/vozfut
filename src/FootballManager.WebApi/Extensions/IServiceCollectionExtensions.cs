using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Infrastructure.Extensions;
using FootballManager.WebApi.Providers;
using FootballManager.WebApi.SwaggerConfig;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FootballManager.WebApi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomVersioning()
                    .AddCustomSwagger(configuration)
                    .AddCustomController()
                    .AddCustomCors()
                    .AddCustomIdentity(configuration);

            return services;
        }

        private static IServiceCollection AddCustomVersioning(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddApiVersioning(
                    options =>
                    {
                        // reporting api versions will return the headers
                        // "api-supported-versions" and "api-deprecated-versions"
                        options.ReportApiVersions = true;

                        options.Policies.Sunset(0.9)
                                        .Effective(DateTimeOffset.Now.AddDays(60))
                                        .Link("policy.html")
                                            .Title("Versioning Policy")
                                            .Type("text/html");
                    })
                .AddApiExplorer(
                    options =>
                    {
                        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    });

            return services;
        }

        private static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(x =>
            {
                x.EnableAnnotations();
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            services.AddSwaggerDocumentationWithBearer(configuration, xmlFile, AppContext.BaseDirectory);

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            return services;
        }

        private static IServiceCollection AddSwaggerDocumentationWithBearer(this IServiceCollection services, IConfiguration configuration, string fileName, string baseDirectory)
        {
            var swaggerOptions = configuration.GetOptions<SwaggerOptions>("Swagger");

            return services
               .AddSwaggerGen(options =>
               {
                   options.SwaggerDoc(swaggerOptions.Name, new OpenApiInfo
                   {
                       Title = swaggerOptions.Title ?? "N/A",
                       Version = swaggerOptions.Version ?? "N/A",
                       Description = swaggerOptions.Description ?? "N/A",
                       TermsOfService = new Uri(swaggerOptions.TermsOfService, UriKind.Absolute),
                       Contact = new()
                       {
                           Url = new Uri(swaggerOptions.Contact.Url, UriKind.Absolute),
                           Name = swaggerOptions.Contact.Name ?? "N/A"
                       },
                       License = new()
                       {
                           Url = new Uri(swaggerOptions.License.Url, UriKind.Absolute),
                           Name = swaggerOptions.License.Name ?? "N/A"
                       }
                   });

                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Please enter into field the word 'Bearer' following by space and JWT",
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
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
                            },new List<string>()
                        }
                   });
                   options.DescribeAllParametersInCamelCase();

                   // Set the comments path for the Swagger JSON and UI.
                   var xmlPath = Path.Combine(baseDirectory, fileName);
                   options.IncludeXmlComments(xmlPath);
               });
        }

        private static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAllOrigins",
                    config =>
                    {
                        config
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }

        private static IServiceCollection AddCustomController(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            return services;
        }

        private static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetOptions<JwtOptions>("Jwt");

            services.AddAuthentication("Bearer").AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidAudience = jwtOptions.Audience,
                     ValidIssuer = jwtOptions.Issuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                 };
             });

            return services;
        }
    }
}
