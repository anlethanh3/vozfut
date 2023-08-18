using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Positions.Queries.GetAutoComplete
{
    public record GetAutoCompletePositionQuery(string Search) : IRequest<PaginatedResult<GetAutoCompletePositionDto>>;

    internal class GetAutoCompletePositionQueryHandler : IRequestHandler<GetAutoCompletePositionQuery, PaginatedResult<GetAutoCompletePositionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetAutoCompletePositionQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetAutoCompletePositionDto>> Handle(GetAutoCompletePositionQuery request, CancellationToken cancellationToken)
        {
            var positions = new List<GetAutoCompletePositionDto>();
            var count = 0;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
            var query = $@"SELECT Id, Name, Code
                           FROM Positions
                           WHERE @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search);

                           SELECT Id
                           FROM Positions
                           WHERE @search IS NULL OR(Name LIKE @search OR Code LIKE @search OR Description LIKE @search);";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                search = "%" + request.Search + "%"
            }))
            {
                positions = (await multipleResut.ReadAsync<GetAutoCompletePositionDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetAutoCompletePositionDto>()).Count();
            }

            return PaginatedResult<GetAutoCompletePositionDto>.Create(positions.ToList(), count, 1, count);
        }
    }
}
