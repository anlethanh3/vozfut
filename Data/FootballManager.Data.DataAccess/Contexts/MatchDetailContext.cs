using Dapper;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Responses;
using Microsoft.Extensions.Logging;

namespace FootballManager.Data.DataAccess.Contexts;
public class MatchDetailContext : IMatchDetailContext
{
    private readonly ILogger<MatchDetailContext> _logger;
    private readonly IDatabaseContext _dbContext;

    public MatchDetailContext(ILogger<MatchDetailContext> logger, IDatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<MatchDetailResponse>> GetAllAsync(int matchId)
    {
        using (var connection = _dbContext.GetDbConnection())
        {
            IEnumerable<MatchDetailResponse> result;
            connection.Open();
            var sql = @"SELECT md.*, m.Name as MatchName, mem.Name as MemberName, mem.Elo as MemberElo
                            FROM ([MatchDetails] as md JOIN [Matches] as m ON md.MatchId = m.Id) 
                                    JOIN [Members] as mem ON md.MemberId = mem.Id
                            WHERE   m.IsDeleted = @IsDeleted AND 
                                    md.IsDeleted = @IsDeleted AND
                                    m.Id = @MatchId";
            result = await connection.QueryAsync<MatchDetailResponse>(sql, new { IsDeleted = 0, MatchId = matchId });
            connection.Close();
            return result;
        }
    }

    public async Task<bool> UpdateAsync(MatchDetail detail)
    {
        var result = false;
        using (var connection = _dbContext.GetDbConnection())
        {
            connection.Open();
            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    var sql = @"UPDATE [MatchDetails] SET 
                            MatchId = @MatchId, MemberId = @MemberId, 
                            IsPaid = @IsPaid, IsSkip = @IsSkip, 
                            ModifiedDate = @ModifiedDate, IsDeleted = @IsDeleted
                            WHERE Id = @Id";
                    var value = await connection.ExecuteAsync(sql, new
                    {
                        Id = detail.Id,
                        MatchId = detail.MatchId,
                        MemberId = detail.MemberId,
                        IsPaid = detail.IsPaid,
                        IsSkip = detail.IsSkip,
                        ModifiedDate = detail.ModifiedDate,
                        IsDeleted = detail.IsDeleted,
                    }, tran);
                    if (value > 0)
                    {
                        tran.Commit();
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    tran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
    }
}