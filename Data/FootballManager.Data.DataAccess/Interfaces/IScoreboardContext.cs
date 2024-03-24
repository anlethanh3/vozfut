using FootballManager.Data.Entity.Entities;

namespace FootballManager.Data.DataAccess.Interfaces;

public interface IScoreboardContext{
    public Task<IEnumerable<Leaderboard>> GetLeaderBoardAsync();
    public Task<IEnumerable<WinnerTeamSize>> GetWinnerAsync(int teamSize);

}