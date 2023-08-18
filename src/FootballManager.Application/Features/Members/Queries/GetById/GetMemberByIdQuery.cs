using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Queries.GetById
{
    public record GetMemberByIdQuery(int Id) : IRequest<Result<GetMemberByIdDto>>;

    internal class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Result<GetMemberByIdDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetMemberByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory, ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetMemberByIdDto>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
            var query = $@"SELECT m.Id, m.Name, m.Elo, m.Description, m.CreatedDate, m.CreatedBy,
                                  p.Id, p.Name, p.Code
                           FROM Members AS m
                           LEFT JOIN Positions AS p ON m.PositionId = p.Id
                           WHERE m.Id = @id AND m.IsDeleted = 0";

            var data = (await connection.QueryAsync<GetMemberByIdDto, GetMemberPositionDto, GetMemberByIdDto>(query, (member, position) =>
            {
                if (position != null)
                {
                    member.Position = position;
                }
                return member;
            }, new { id = request.Id },
            splitOn: "Id, Id")).FirstOrDefault();

            return data != null ? Result<GetMemberByIdDto>.Success(data) : Result<GetMemberByIdDto>.Failure();
        }
    }
}
