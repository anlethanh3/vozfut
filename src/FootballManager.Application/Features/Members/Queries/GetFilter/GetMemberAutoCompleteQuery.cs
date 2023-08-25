using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Queries.GetFilter
{
    public record GetMemberAutoCompleteQuery(string Search) : IRequest<PaginatedResult<GetMemberAutoCompleteDto>>;

    internal class GetMemberAutoCompleteQueryHandler : IRequestHandler<GetMemberAutoCompleteQuery, PaginatedResult<GetMemberAutoCompleteDto>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly string _connectionString;

        public GetMemberAutoCompleteQueryHandler(ISqlConnectionFactory connectionFactory,
            ConnectionOptions connectionOptions)
        {
            _connectionFactory = connectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetMemberAutoCompleteDto>> Handle(GetMemberAutoCompleteQuery request, CancellationToken cancellationToken)
        {
            var members = new List<GetMemberAutoCompleteDto>();
            var count = 0;
            using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString);
            var query = $@"SELECT Id, Name, Description
                           FROM Members
                           WHERE @search IS NULL OR(Name LIKE @search OR Description LIKE @search)
                                  AND IsDeleted = 0;
                           SELECT Id
                           FROM Members
                           WHERE @search IS NULL OR(Name LIKE @search OR Description LIKE @search);";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                search = "%" + request.Search + "%"
            }))
            {
                members = (await multipleResut.ReadAsync<GetMemberAutoCompleteDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetMemberAutoCompleteDto>()).Count();
            }

            return PaginatedResult<GetMemberAutoCompleteDto>.Create(members, count, 1, count);
        }
    }
}
