using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Queries.GetPaging
{
    public record GetPagingVoteQuery(int Page, int Limit, string Search, string Sort) : IRequest<PaginatedResult<GetPagingVoteDto>>;

    internal class GetPagingVoteQueryHandler : IRequestHandler<GetPagingVoteQuery, PaginatedResult<GetPagingVoteDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetPagingVoteQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetPagingVoteDto>> Handle(GetPagingVoteQuery request, CancellationToken cancellationToken)
        {
            var votes = new List<GetPagingVoteDto>();
            var count = 0;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
            var offset = (request.Page - 1) * request.Limit;
            var query = $@"SELECT Id, Name, Code, Status, Description, CreatedBy, CreatedDate
                           FROM Votes
                           WHERE @search IS NULL OR(Name LIKE @search OR Description LIKE @search OR Code LIKE @search)
                                 AND IsDeleted = 0
                           ORDER BY Id DESC
                           OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;

                           SELECT Id
                           FROM Votes
                           WHERE @search IS NULL OR(Name LIKE @search OR Description LIKE @search OR Code LIKE @search)
                                 AND IsDeleted = 0;";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                limit = request.Limit,
                offset,
                search = "%" + request.Search + "%"
            }))
            {
                votes = (await multipleResut.ReadAsync<GetPagingVoteDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetPagingVoteDto>()).Count();
            }

            return PaginatedResult<GetPagingVoteDto>.Create(votes, count, request.Page, request.Limit);
        }
    }
}
