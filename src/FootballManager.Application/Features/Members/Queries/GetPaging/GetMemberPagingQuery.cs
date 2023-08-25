using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Queries.GetPaging
{
    public record GetMemberPagingQuery(int Page, int Limit, string Search, string Sort) : IRequest<PaginatedResult<GetMemberPagingDto>>;

    internal class GetMemberPagingQueryHandler : IRequestHandler<GetMemberPagingQuery, PaginatedResult<GetMemberPagingDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetMemberPagingQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<PaginatedResult<GetMemberPagingDto>> Handle(GetMemberPagingQuery request, CancellationToken cancellationToken)
        {
            var count = 0;
            var offset = (request.Page - 1) * request.Limit;
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var query = $@"SELECT m.Id, m.Name, m.Description, m.CreatedBy, m.CreatedDate,
                                  p.Id, p.Name, p.Code,
                                  sp.Id, sp.Name, sp.Code
                           FROM Members AS m
                            LEFT JOIN Positions AS p ON m.PositionId = p.Id
                            LEFT JOIN Positions AS sp ON m.SubPositionId = sp.Id
                           WHERE @search IS NULL OR (m.Name LIKE @search OR m.Description LIKE @search)
                            AND m.IsDeleted = 0
                            ORDER BY m.Id DESC
                           OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

            var queryCount = $@"SELECT COUNT(m.Id)
                               FROM Members AS m
                                LEFT JOIN Positions AS p ON m.PositionId = p.Id
                                LEFT JOIN Positions AS sp ON m.SubPositionId = sp.Id
                               WHERE @search IS NULL OR(m.Name LIKE @search OR m.Description LIKE @search)
                                AND m.IsDeleted = 0; ";

            var data = await connection.QueryAsync<GetMemberPagingDto, GetMemberPagingPositionDto, GetMemberPagingSubPositionDto, GetMemberPagingDto>(query, (member, position, subPosition) =>
            {
                if (position != null)
                {
                    member.Position = position;
                }

                if (subPosition != null)
                {
                    member.SubPosition = subPosition;
                }

                return member;
            }, new
            {
                offset,
                limit = request.Limit,
                search = "%" + request.Search + "%",
                sort = request.Sort
            }, splitOn: "Id");

            count = await connection.QueryFirstAsync<int>(queryCount, new
            {
                search = "%" + request.Search + "%"
            });

            return PaginatedResult<GetMemberPagingDto>.Create(data.ToList(), count, request.Page, request.Limit);
        }
    }
}
