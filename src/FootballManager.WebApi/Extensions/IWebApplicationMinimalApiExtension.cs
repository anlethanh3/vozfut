using FootballManager.Application.Features.Matches.Commands.Create;
using FootballManager.Application.Features.Matches.Commands.Delete;
using FootballManager.Application.Features.Matches.Commands.Update;
using FootballManager.Application.Features.Matches.Commands.UpdateStatus;
using FootballManager.Application.Features.Matches.Queries.GetAll;
using FootballManager.Application.Features.Matches.Queries.GetDetail;
using FootballManager.Application.Features.Matches.Queries.GetPaging;
using FootballManager.Application.Features.Users.Commands.Authenticate;
using FootballManager.Application.Features.Users.Commands.Create;
using FootballManager.Application.Features.Users.Commands.Update;
using FootballManager.Application.Features.Users.Queries.GetAll;
using FootballManager.Application.Features.Users.Queries.Profile;
using FootballManager.Domain.Enums;
using FootballManager.Domain.ResultModels;
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
            return app.UseV1UserBuilder()
                      .UseV1MatchBuilder();
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

            usersV1.MapGet("/profile", async (IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(new GetProfileUserQuery()));
            }).WithMetadata(new SwaggerOperationAttribute("Endpoint using for get user information."));

            usersV1.MapGet("/", async (IMediator mediator, int page, int limit) =>
            {
                return Results.Ok(await mediator.Send(new GetAllUserQuery(page, limit)));
            }).WithMetadata(new SwaggerOperationAttribute("Endpoint using for get paging user information."));

            return app;
        }

        private static WebApplication UseV1MatchBuilder(this WebApplication app)
        {
            var matches = app.NewVersionedApi("Matches");
            var matchesV1 = matches.MapGroup("/api/v{version:apiVersion}/matches").RequireAuthorization();

            matchesV1.MapPost("/", async (IMediator mediator, [FromBody] CreateMatchCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for create new matches"));

            matchesV1.MapPut("/", async (IMediator mediator, [FromBody] UpdateMatchCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for update matches information"));

            matchesV1.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                return Results.Ok(await mediator.Send(new DeleteMatchCommand(id)));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for delete matches information"));

            matchesV1.MapPut("/update-status", async (IMediator mediator, [FromBody] UpdateStatusMatchCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for update status matches information"));

            matchesV1.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                return Results.Ok(await mediator.Send(new GetDetailMatchQuery(id)));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get detail matches information"));

            matchesV1.MapGet("/", async (IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(new GetAllMatchQuery()));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get all matches information"));

            matchesV1.MapGet("/paging", async (IMediator mediator, int page, int limit) =>
            {
                return Results.Ok(await mediator.Send(new GetPagingMatchQuery(page, limit)));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get paging matches information"));

            matchesV1.MapGet("/status", async () =>
            {
                return Results.Ok(await Result<IEnumerable<MatchStatusEnum>>.SuccessAsync(MatchStatusEnum.Gets));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for gets status matches information"));

            matchesV1.MapGet("/status/{id:int}", async (int id) =>
            {
                return Results.Ok(await Result<MatchStatusEnum>.SuccessAsync(MatchStatusEnum.Get((short)id)));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get detail status matches information"));

            return app;
        }
    }
}
