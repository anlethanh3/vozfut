using FootballManager.Application.Features.Users.Commands.Authenticate;
using FootballManager.Application.Features.Users.Commands.Create;
using FootballManager.Application.Features.Users.Commands.Update;
using FootballManager.Application.Features.Users.Queries.GetAll;
using FootballManager.Application.Features.Users.Queries.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FootballManager.WebApi.Extensions
{
    public static class IWebApplicationMinimalApiExtension
    {
        /// <summary>
        /// Minimal APIs
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseMinimalApi(this WebApplication app)
        {
            return app.UseV1UserBuilder();
        }

        private static WebApplication UseV1UserBuilder(this WebApplication app)
        {
            var users = app.NewVersionedApi("Users");
            var usersV1 = users.MapGroup("/api/v{version:apiVersion}/users").RequireAuthorization();

            usersV1.MapPost("/", async (IMediator mediator, [FromBody] CreateUserCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for register new user"));

            usersV1.MapPost("/authenticate", async (IMediator mediator, [FromBody] AuthenticateUserCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .AllowAnonymous()
            .WithMetadata(new SwaggerOperationAttribute("This endpoint using for get token"));

            usersV1.MapPut("/", async (IMediator mediator, [FromBody] UpdateUserCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            }).WithMetadata(new SwaggerOperationAttribute("Endpoint using for update user information."));

            usersV1.MapGet("/profile", async (IMediator mediator, GetProfileUserQuery query) =>
            {
                return Results.Ok(await mediator.Send(query));
            }).WithMetadata(new SwaggerOperationAttribute("Endpoint using for get user information."));

            usersV1.MapGet("/", async (IMediator mediator, GetAllUserQuery query) =>
            {
                return Results.Ok(await mediator.Send(query));
            }).WithMetadata(new SwaggerOperationAttribute("Endpoint using for get paging user information."));

            return app;
        }
    }
}
