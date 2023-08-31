using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Queries.GetTeam
{
    public record GetTeamByMatchQuery(int MatchId) : IRequest<Result<GetTeamByMatchDto>>;

    internal class GetTeamByMatchQueryHandler : IRequestHandler<GetTeamByMatchQuery, Result<GetTeamByMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetTeamByMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetTeamByMatchDto>> Handle(GetTeamByMatchQuery request, CancellationToken cancellationToken)
        {
            var matchDetail = new GetTeamByMatchDto();
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var queryMatch = $@"SELECT Id, VoteId, TeamSize, TeamCount FROM Matches WHERE IsDeleted = 0 AND Id = @match_id";
            var match = await connection.QueryFirstOrDefaultAsync<MatchDto>(queryMatch, new
            {
                match_id = request.MatchId
            }) ?? throw new DomainException("Match not found");

            var query = $@"SELECT m.Id, m.Name, md.Id, md.BibColour
                           FROM Matches AS m
                            LEFT JOIN MatchDetails AS md ON m.Id = md.MatchId
                           WHERE m.IsDeleted = 0 AND m.Id = @match_id";

            var data = await connection.QueryAsync<GetTeamByMatchDto, BibColourTeamDto, GetTeamByMatchDto>(query, (match, matchDetail) =>
            {
                if (matchDetail != null)
                {
                    match.Teams.Add(matchDetail);
                }
                return match;
            }, new { match_id = request.MatchId }, splitOn: "Id");

            //var groupedData = data
            //.SelectMany(match => match.Teams) // Gộp tất cả đội từ tất cả trận đấu
            //.GroupBy(team => team.BibColour) // Nhóm theo trường BibColour
            //.ToDictionary(group => group.Key, group => group.ToList());

            matchDetail.Id = data.FirstOrDefault().Id;
            matchDetail.Name = data.FirstOrDefault().Name;
            var idCounter = 1;
            var groupedData = data.SelectMany(match => match.Teams)
                                  .GroupBy(team => team.BibColour)
                                  .Select(group => group.Key)
                                  .ToList();

            matchDetail.Teams = groupedData.Select(x => new BibColourTeamDto
            {
                Id = idCounter++,
                BibColour = x,
            }).ToList();

            return Result<GetTeamByMatchDto>.Success(matchDetail);
        }
    }
}
