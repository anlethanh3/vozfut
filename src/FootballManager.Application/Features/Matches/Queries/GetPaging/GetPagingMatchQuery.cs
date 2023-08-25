using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetPaging
{
    public record GetPagingMatchQuery(int Page, int Limit, string Search, string Sort, DateTime? StartDate, DateTime? EndDate) : IRequest<PaginatedResult<GetPagingMatchDto>>;

    internal class GetPagingMatchQueryHandler : IRequestHandler<GetPagingMatchQuery, PaginatedResult<GetPagingMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetPagingMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetPagingMatchDto>> Handle(GetPagingMatchQuery request, CancellationToken cancellationToken)
        {
            var matches = new List<GetPagingMatchDto>();
            var count = 0;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var offset = (request.Page - 1) * request.Limit;

            //if (request.EndDate <= request.StartDate)
            //{
            //    throw new DomainException("End date must greater than start date");
            //}

            var query = $@"SELECT Id, Name, Code, Description, TeamSize, TeamCount, TotalAmount, TotalHour,
                                  FootballFieldSize, FootballFieldAddress, FootballFieldNumber,
                                  MatchDate, StartTime, EndTime, Status, CreatedDate, CreatedBy
                           FROM Matches
                           WHERE IsDeleted = 0
                                    AND @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search OR FootballFieldAddress LIKE @search)
                                    AND @start_date IS NULL OR (MatchDate >= @start_date)
                                    AND @end_date IS NULL OR (MatchDate <= @end_date)
                           ORDER BY Id DESC
                            OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;

                           SELECT Id
                           FROM Matches
                           WHERE IsDeleted = 0
                                    AND @search IS NULL OR (Name LIKE @search OR Code LIKE @search OR Description LIKE @search OR FootballFieldAddress LIKE @search)
                                    AND @start_date IS NULL OR (MatchDate >= @start_date)
                                    AND @end_date IS NULL OR (MatchDate <= @end_date);";

            using (var multipleResut = await connection.QueryMultipleAsync(query, new
            {
                search = "%" + request.Search + "%",
                offset,
                limit = request.Limit,
                sort = request.Sort,
                start_date = request.StartDate,
                end_date = request.EndDate
            }))
            {
                matches = (await multipleResut.ReadAsync<GetPagingMatchDto>()).ToList();
                count = (await multipleResut.ReadAsync<GetPagingMatchDto>()).Count();
            }

            return PaginatedResult<GetPagingMatchDto>.Create(matches, count, request.Page, request.Limit);
        }
    }
}
