using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Positions.Queries.GetPaging
{
    public record GetPagingPositionQuery(int Page, int Limit, string Search, string Sort) : IRequest<PaginatedResult<GetPagingPositionDto>>;

    internal class GetPagingPositionQueryHandler : IRequestHandler<GetPagingPositionQuery, PaginatedResult<GetPagingPositionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetPagingPositionQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetPagingPositionDto>> Handle(GetPagingPositionQuery request, CancellationToken cancellationToken)
        {
            var positions = new List<GetPagingPositionDto>();
            int count = 0;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
            var offset = (request.Page - 1) * request.Limit;

            var query = $@"SELECT Id, Name, Code, Description, CreatedDate, CreatedBy
                           FROM Positions
                           WHERE @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search)
                            ORDER BY Id DESC
                           OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;

                           SELECT Id
                           FROM Positions
                           WHERE @search IS NULL OR(Name LIKE @search OR Code LIKE @search OR Description LIKE @search);";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                limit = request.Limit,
                offset,
                search = "%" + request.Search + "%"
            }))
            {
                positions = (await multipleResut.ReadAsync<GetPagingPositionDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetPagingPositionDto>()).Count();
            }

            return PaginatedResult<GetPagingPositionDto>.Create(positions, count, request.Page, request.Limit);
        }
    }
}
