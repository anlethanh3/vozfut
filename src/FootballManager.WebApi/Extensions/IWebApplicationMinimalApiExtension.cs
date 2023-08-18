using FootballManager.Application.Features.MatchDetails.Commands.Create;
using FootballManager.Application.Features.MatchDetails.Commands.Update;
using FootballManager.Application.Features.Matches.Commands.Create;
using FootballManager.Application.Features.Matches.Commands.Delete;
using FootballManager.Application.Features.Matches.Commands.Update;
using FootballManager.Application.Features.Matches.Commands.UpdateStatus;
using FootballManager.Application.Features.Matches.Queries.GetAll;
using FootballManager.Application.Features.Matches.Queries.GetDetail;
using FootballManager.Application.Features.Matches.Queries.GetPaging;
using FootballManager.Application.Features.Members.Commands.Create;
using FootballManager.Application.Features.Members.Commands.Delete;
using FootballManager.Application.Features.Members.Commands.Update;
using FootballManager.Application.Features.Members.Queries.GetById;
using FootballManager.Application.Features.Positions.Commands.Create;
using FootballManager.Application.Features.Positions.Commands.Update;
using FootballManager.Application.Features.Positions.Queries.GetAutoComplete;
using FootballManager.Application.Features.Positions.Queries.GetDetail;
using FootballManager.Application.Features.Positions.Queries.GetPaging;
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
                      .UseV1MatchBuilder()
                      .UseV1MatchDeatilBuilder()
                      .UseV1MemberBuilder()
                      .UseV1PositionBuilder();
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

        private static WebApplication UseV1MatchDeatilBuilder(this WebApplication app)
        {
            var matchDetails = app.NewVersionedApi("MatchDetails");
            var matcheDetailsV1 = matchDetails.MapGroup("/api/v{version:apiVersion}/match-details").RequireAuthorization();

            matcheDetailsV1.MapPost("/", async (IMediator mediator, [FromBody] CreateMatchDetailCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for create match detail"));

            matcheDetailsV1.MapPut("/", async (IMediator mediator, [FromBody] UpdateMatchDetailCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for update match detail"));

            return app;
        }

        private static WebApplication UseV1MemberBuilder(this WebApplication app)
        {
            var members = app.NewVersionedApi("Members");
            var membersV1 = members.MapGroup("/api/v{version:apiVersion}/members").RequireAuthorization();

            membersV1.MapPost("/", async (IMediator mediator, [FromBody] CreateMemberCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for create member"));

            membersV1.MapPut("/", async (IMediator mediator, [FromBody] UpdateMemberCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for update member"));

            membersV1.MapDelete("/", async (IMediator mediator, [FromBody] DeleteMemberCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for delete member"));

            membersV1.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                return Results.Ok(await mediator.Send(new GetMemberByIdQuery(id)));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get detail member"));

            return app;
        }

        private static WebApplication UseV1PositionBuilder(this WebApplication app)
        {
            var positions = app.NewVersionedApi("Positions");
            var positionsV1 = positions.MapGroup("/api/v{version:apiVersion}/positions").RequireAuthorization();

            positionsV1.MapPost("/", async (IMediator mediator, [FromBody] CreatePositionCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for create positions"));

            positionsV1.MapPut("/", async (IMediator mediator, [FromBody] UpdatePositionCommand command) =>
            {
                return Results.Ok(await mediator.Send(command));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for update positions"));

            positionsV1.MapGet("/", async (IMediator mediator, int page, int limit, string search, string sort) =>
            {
                return Results.Ok(await mediator.Send(new GetPagingPositionQuery(page, limit, search, sort)));
            })
            .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get paging positions"));

            positionsV1.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                return Results.Ok(await mediator.Send(new GetDetailPositionQuery(id)));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get detail positions"));

            positionsV1.MapGet("/filter", async (IMediator mediator, string search) =>
            {
                return Results.Ok(await mediator.Send(new GetAutoCompletePositionQuery(search)));
            })
           .WithMetadata(new SwaggerOperationAttribute("Endpoint using for get auto-complete positions"));

            return app;
        }
    }
}
