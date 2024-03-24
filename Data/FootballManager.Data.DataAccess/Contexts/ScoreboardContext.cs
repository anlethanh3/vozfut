using Dapper;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Responses;
using Microsoft.Extensions.Logging;

namespace FootballManager.Data.DataAccess.Contexts;
public class ScoreboardContext : IScoreboardContext
{
    private readonly ILogger<ScoreboardContext> _logger;
    private readonly IDatabaseContext _dbContext;

    public ScoreboardContext(ILogger<ScoreboardContext> logger, IDatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Leaderboard>> GetLeaderBoardAsync()
    {
        using (var connection = _dbContext.GetDbConnection())
        {
            connection.Open();
            /*var sql = @"SELECT md.Id as MatchDetailId,m.Id as MemberId,mt.Id as MatchId, md.IsWinner, mt.TeamSize, mt.TeamCount ,SUM(md.Goal) AS Goal, SUM(md.Assist) as Assist
                        FROM ([MatchDetails] as md JOIN [Members] as m ON md.MemberId = m.Id) JOIN [Matches] as mt ON md.MatchId = mt.Id
                        WHERE m.IsDeleted = @IsDeleted AND md.IsDeleted = @IsDeleted AND mt.IsDeleted= @IsDeleted AND md.IsPaid=@IsPaid AND( md.Goal>0 OR md.Assist>0)
                        GROUP BY md.Id,m.Id,md.IsWinner,mt.TeamSize, mt.TeamCount,mt.Id";*/
            var sql = @"SELECT m.Id as MemberId, m.Name as MemberName, SUM(md.Goal) AS Goal, SUM(md.Assist) as Assist
                                FROM ([MatchDetails] as md JOIN [Members] as m ON md.MemberId = m.Id)
                                WHERE m.IsDeleted = @IsDeleted AND md.IsDeleted = @IsDeleted  AND md.IsPaid=@IsPaid AND( md.Goal>0 OR md.Assist>0)
                                GROUP BY m.Id, m.Name";
            var result = await connection.QueryAsync<Leaderboard>(sql, new { IsDeleted = 0, IsPaid = 1 });
            connection.Close();
            return result;
        }
    }
    public async Task<IEnumerable<WinnerTeamSize>> GetWinnerAsync(int teamSize)
    {
        using (var connection = _dbContext.GetDbConnection())
        {
            connection.Open();
            var sql = @"SELECT m.Id as MemberId,m.Name as MemberName,mt.TeamSize, Count(md.IsWinner) as Winner
                    FROM ([MatchDetails] as md JOIN [Members] as m ON md.MemberId = m.Id) JOIN [Matches] as mt ON md.MatchId = mt.Id
                    WHERE m.IsDeleted = @IsDeleted AND md.IsDeleted = @IsDeleted AND mt.IsDeleted= @IsDeleted AND md.IsPaid=@IsPaid AND md.IsWinner=@IsWinner AND mt.TeamSize=@TeamSize
                    GROUP BY m.Id, m.Name,mt.TeamSize";
            var result = await connection.QueryAsync<WinnerTeamSize>(sql, new { IsDeleted = 0, IsPaid = 1, IsWinner = 1, TeamSize = teamSize });
            connection.Close();
            return result;
        }
    }
}