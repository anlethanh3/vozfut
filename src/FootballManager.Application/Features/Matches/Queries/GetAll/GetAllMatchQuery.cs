using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetAll
{
    public record GetAllMatchQuery(string Search) : IRequest<PaginatedResult<GetAllMatchDto>>;

    internal class GetAllMatchQueryHandler : IRequestHandler<GetAllMatchQuery, PaginatedResult<GetAllMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetAllMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetAllMatchDto>> Handle(GetAllMatchQuery request, CancellationToken cancellationToken)
        {
            var matches = new List<GetAllMatchDto>();
            var count = 0;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
            var query = $@"SELECT Id, Name, Code, Description
                           FROM Matches
                           WHERE @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search)
                                AND IsDeleted = 0;

                           SELECT Id
                           FROM Matches
                           WHERE @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search)
                                AND IsDeleted = 0;";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                search = "%" + request.Search + "%"
            }))
            {
                matches = (await multipleResut.ReadAsync<GetAllMatchDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetAllMatchDto>()).Count();
            }

            if (count == 0)
            {
                count = 10;
            }

            return PaginatedResult<GetAllMatchDto>.Create(matches, count, 1, count);
        }
    }
}
