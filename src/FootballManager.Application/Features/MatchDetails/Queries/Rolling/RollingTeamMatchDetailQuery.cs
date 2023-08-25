using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Enums;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MatchDetails.Queries.Rolling
{
    public record RollingTeamMatchDetailQuery(int MatchId) : IRequest<PaginatedResult<RollingTeamMatchDetailDto>>;

    internal class RollingTeamMatchDetailQueryHandler : IRequestHandler<RollingTeamMatchDetailQuery, PaginatedResult<RollingTeamMatchDetailDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;
        private readonly BibColourOption _bibColourOption;

        public RollingTeamMatchDetailQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions,
            BibColourOption bibColourOption)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
            _bibColourOption = bibColourOption;
        }

        public async Task<PaginatedResult<RollingTeamMatchDetailDto>> Handle(RollingTeamMatchDetailQuery request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var queryMatch = $@"SELECT Id, VoteId, TeamSize, TeamCount FROM Matches WHERE IsDeleted = 0 AND Id = @match_id AND Status = @status";
            var match = await connection.QueryFirstOrDefaultAsync<MatchDto>(queryMatch, new
            {
                match_id = request.MatchId,
                status = MatchStatusEnum.ComingSoon.Name
            }) ?? throw new DomainException("Match not found");

            // Lấy danh sách những member nào tham gia vote để chuẩn bị roll
            var queryMember = $@"SELECT mv.MemberId, mv.VoteId, mv.VoteDate, mv.IsJoin,
                                        m.Id, m.Name, m.Elo
                                 FROM MemberVotes AS mv
                                    JOIN Members AS m ON mv.MemberId = m.Id
                                WHERE mv.VoteId = @vote_id AND mv.IsJoin = 1 AND m.IsDeleted = 0
                                ORDER BY mv.VoteDate DESC";

            var members = await connection.QueryAsync<RollingTeamMatchDetailMemberDto>(queryMember, new
            {
                vote_id = match.VoteId
            });

            // Tiến hành chia team random
            var teamSize = match.TeamSize; // số lượng thành viên trong đội
            var teamCount = match.TeamCount; // số đội tham gia

            var rnd = new Random();
            var orderElo = members.Take(teamCount * teamSize).OrderByDescending(item => item.Elo).ToList();
            var teams = new List<RollingTeamMatchDetailMemberDto>[match.TeamCount];

            for (var i = 0; i < match.TeamSize; i++)
            {
                var random = orderElo.Take(match.TeamCount).OrderBy(x => rnd.Next()).ToList();
                for (var j = 0; j < match.TeamCount; j++)
                {
                    if (teams[j] is null)
                    {
                        teams[j] = new();
                    }
                    teams[j].Add(random[j]);
                    orderElo.Remove(random[j]);
                }
            }

            var colors = new List<string>();
            colors.AddRange(_bibColourOption.BibColour);

            var result = teams.Select(x => new RollingTeamMatchDetailDto
            {
                Members = x,
                TotalElo = (short)x.Sum(m => m.Elo),
            }).ToList();

            foreach (var item in result)
            {
                var randomIndex = rnd.Next(colors.Count);
                var randomColor = colors[randomIndex];
                item.BibColour = randomColor;
                colors.RemoveAt(randomIndex);
            }

            return PaginatedResult<RollingTeamMatchDetailDto>.Create(result, result.Count, 1, result.Count);
        }
    }
}
