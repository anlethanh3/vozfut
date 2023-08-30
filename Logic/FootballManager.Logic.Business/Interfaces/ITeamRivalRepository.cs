using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface ITeamRivalRepository
{
    public Task<IEnumerable<TeamRival>> CreateAsync(int id);
    public Task<IEnumerable<TeamRival>> GetAsync(int id);
    public Task<bool> SaveAsync(int matchId, IEnumerable<TeamRival> teams);
}