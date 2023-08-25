using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Queries.GetFilter
{
    public class GetVoteFilterQuery : IRequest<PaginatedResult<GetVoteFilterDto>>
    {
        public string Search { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    };

    internal class GetVoteFilterQueryHandler : IRequestHandler<GetVoteFilterQuery, PaginatedResult<GetVoteFilterDto>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly string _connectionString;

        public GetVoteFilterQueryHandler(ISqlConnectionFactory connectionFactory,
            ConnectionOptions connectionOptions)
        {
            _connectionFactory = connectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetVoteFilterDto>> Handle(GetVoteFilterQuery request, CancellationToken cancellationToken)
        {
            var startDate = request.Start;
            var endDate = request.End;

            var count = 0;

            if (startDate.HasValue && endDate == null)
            {
                endDate = startDate.Value.AddMonths(1);
            }
            else if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new DomainException("Start must be smaller than end");
            }

            using var connection = await _connectionFactory.CreateConnectionAsync(_connectionString);
            var query = $@"SELECT Id, Name, Code, Status
                           FROM Votes
                           WHERE IsDeleted = 0
                                AND @search IS NULL OR(Name LIKE @search OR Code LIKE @search)
                                AND (@start IS NULL OR CreatedDate >= @start)
                                AND (@end IS NULL OR CreatedDate <= @end)
                            ORDER BY Id DESC;";

            var data = await connection.QueryAsync<GetVoteFilterDto>(query, new
            {
                search = "%" + request.Search + "%",
                start = startDate,
                end = endDate
            });

            count = data.Count();

            if (!data.Any())
            {
                count = 0;
            }

            return PaginatedResult<GetVoteFilterDto>.Create(data.ToList(), count, 1, count);
        }
    }
}
