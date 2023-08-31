using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchScores.Queries.GetByMatch
{
    public record GetMatchScoreByMatchQuery(int MatchId) : IRequest<Result<GetMatchScoreByMatchDto>>;

    internal class GetMatchScoreByMatchQueryHandler : IRequestHandler<GetMatchScoreByMatchQuery, Result<GetMatchScoreByMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetMatchScoreByMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetMatchScoreByMatchDto>> Handle(GetMatchScoreByMatchQuery request, CancellationToken cancellationToken)
        {
            var matchScore = new GetMatchScoreByMatchDto();
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var query = $@"SELECT m.Id AS MatchId,
                                  ms.Id, ms.MatchId, ms.Team1, ms.Team2, ms.NumberGoalTeam1, ms.NumberGoalTeam2, ms.CreatedDate, ms.CreatedBy
                           FROM Matches AS m
                            JOIN MatchScores AS ms ON m.Id = ms.MatchId
                           WHERE ms.MatchId = @match_id AND m.IsDeleted = 0";

            var data = (await connection.QueryAsync<GetMatchScoreByMatchDto, MatchScheduleDto, GetMatchScoreByMatchDto>(query, (match, matchScore) =>
            {
                if (matchScore != null)
                {
                    match.Schedules.Add(matchScore);
                }
                return match;
            }, new { match_id = request.MatchId }));

            matchScore = data.GroupBy(x => x.MatchId).Select(x =>
            {
                matchScore = x.First();
                if (matchScore.Schedules.Any())
                {
                    matchScore.Schedules = x.SelectMany(y => y.Schedules).OrderBy(x => x.Id).ToList();
                }
                return matchScore;
            }).FirstOrDefault();

            return Result<GetMatchScoreByMatchDto>.Success(matchScore);
        }
    }
}
