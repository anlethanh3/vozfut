namespace FootballManager.Logic.Business.Interfaces
{
    public interface IRollRepoitory
    {
        Task<bool> TeamAssignmentAsync(int numPlayers, int numTeams);
    }
}