using System.Data;
using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public record GetDetailMatchQuery(int Id) : IRequest<Result<GetDetailMatchDto>>;

    internal class GetDetailMatchQueryHandler : IRequestHandler<GetDetailMatchQuery, Result<GetDetailMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly string _connectionString;

        public GetDetailMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetDetailMatchDto>> Handle(GetDetailMatchQuery request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var matchExists = await CheckIfMatchExists(connection, request.Id);
            if (!matchExists)
            {
                throw new DomainException("Match not found");
            }

            var data = await LoadMatchDetails(connection, request.Id);

            var teams = GroupMembersByTeam(data);

            var matchDetail = ExtractMatchDetail(data);

            matchDetail.Teams = teams;

            return Result<GetDetailMatchDto>.Success(matchDetail);
        }

        private async Task<bool> CheckIfMatchExists(IDbConnection connection, int matchId)
        {
            var query = "SELECT COUNT(1) FROM Matches WHERE Id = @match_id AND IsDeleted = 0";
            var result = await connection.QueryFirstAsync<int>(query, new { match_id = matchId });
            return result != 0;
        }

        private async Task<IEnumerable<GetDetailMatchDto>> LoadMatchDetails(IDbConnection connection, int matchId)
        {
            var queryJoin = $@"SELECT m.Id, m.Name, m.Code, m.TeamSize, m.TeamCount, m.TotalAmount, m.TotalHour, m.FootballFieldSize, m.FootballFieldAddress, m.FootballFieldNumber,
                                  m.MatchDate, m.Description, m.StartTime, m.EndTime, m.Status, m.CreatedDate, m.CreatedBy,
                                  md.Id, md.MemberId, md.MatchId, md.IsPaid, md.IsSkip, md.BibColour, md.CreatedDate, md.CreatedBy,
                                  me.Id, me.Name, me.Description, me.Elo,
                                  v.Id, v.Name, v.Code, v.Status
                           FROM Matches AS m
                            LEFT JOIN MatchDetails AS md ON m.Id = md.MatchId
                            LEFT JOIN Members AS me ON md.MemberId = me.Id
                            LEFT JOIN Votes AS v ON m.VoteId = v.Id
                           WHERE m.Id = @match_id AND m.IsDeleted = 0";

            var data = await connection.QueryAsync<GetDetailMatchDto, MatchDetailDto, GetDetailMatchMemberDto, GetDetailMatchVoteDto, GetDetailMatchDto>(queryJoin,
               (match, matchDetail, member, vote) =>
               {
                   if (member != null)
                   {
                       var teamOfMatch = new TeamOfMatchDto
                       {
                           Name = matchDetail.BibColour,
                           Members = new List<MemberOfTeamMatchDto>
                           {
                                new MemberOfTeamMatchDto
                                {
                                    Id = member.Id,
                                    Name = member.Name,
                                    IsPaid = matchDetail.IsPaid,
                                    IsSkip = matchDetail.IsSkip
                                }
                           }
                       };
                       match.Teams.Add(teamOfMatch);
                   }
                   match.Vote = vote;
                   return match;
               }, new { match_id = matchId }, splitOn: "Id, Id, Id, Id");

            return data;
        }

        private List<TeamOfMatchDto> GroupMembersByTeam(IEnumerable<GetDetailMatchDto> data)
        {
            var groupTeams = data
                .SelectMany(team => team.Teams)
                .GroupBy(member => member.Name)
                .ToDictionary(group => group.Key, group => group.ToList());

            var teams = new List<TeamOfMatchDto>();

            foreach (var name in groupTeams.Keys)
            {
                var team = new TeamOfMatchDto
                {
                    Name = name,
                    Members = groupTeams[name].Select(member => new MemberOfTeamMatchDto
                    {
                        Id = member.Members.First().Id,
                        Name = member.Members.First().Name,
                        IsPaid = member.Members.First().IsPaid,
                        IsSkip = member.Members.First().IsSkip
                    }).ToList()
                };
                teams.Add(team);
            }

            return teams;
        }

        private GetDetailMatchDto ExtractMatchDetail(IEnumerable<GetDetailMatchDto> data)
        {
            var matchDetail = data
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .FirstOrDefault();

            return matchDetail;
        }
    }
}
