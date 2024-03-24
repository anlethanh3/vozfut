using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface IScoreboardRepository
{
    public Task<IEnumerable<Leaderboard>> GetLeaderBoardAsync();
    public Task<IEnumerable<WinnerTeamSize>> GetWinnerAsync(int teamSize);
}