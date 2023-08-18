using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Positions.Queries.GetDetail
{
    public record GetDetailPositionQuery(int Id) : IRequest<Result<GetDetailPositionDto>>;

    internal class GetPositionDetailQueryHandler : IRequestHandler<GetDetailPositionQuery, Result<GetDetailPositionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetPositionDetailQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetDetailPositionDto>> Handle(GetDetailPositionQuery request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var query = $@"SELECT Id, Name, Code, Description, CreatedDate, CreatedBy
                           FROM Positions
                           WHERE Id = @id
                           ORDER BY CreatedDate DESC";

            var result = await connection.QueryFirstOrDefaultAsync<GetDetailPositionDto>(query, new { id = request.Id });

            return result != null ? Result<GetDetailPositionDto>.Success(result) : Result<GetDetailPositionDto>.Failure();
        }
    }
}
