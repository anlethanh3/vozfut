using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class ScoreboardRepository : IScoreboardRepository
{
    private readonly EntityDbContext entityDbContext;
    private readonly IScoreboardContext scoreboardContext;

    public ScoreboardRepository(EntityDbContext entityDbContext, IScoreboardContext scoreboardContext)
    {
        this.entityDbContext = entityDbContext;
        this.scoreboardContext = scoreboardContext;
    }

    public async Task<IEnumerable<Leaderboard>> GetLeaderBoardAsync()
    {
        var result = await scoreboardContext.GetLeaderBoardAsync();
        return result.OrderByDescending(x => x.Goal);
    }

    public async Task<IEnumerable<WinnerTeamSize>> GetWinnerAsync(int teamSize)
    {
        var result = await scoreboardContext.GetWinnerAsync(teamSize);
        return result.OrderByDescending(x => x.Winner).ThenBy(x => x.MemberName);
    }
}