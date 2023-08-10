using FootballManager.Data.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface IMatchRepository
{
    public Task<Match?> GetAsync(int id);
    public Task<IEnumerable<Match>> GetAsync();
    public Task<Match?> AddAsync(Match match);
    public Task<IEnumerable<Match>> SearchAsync(string name);
    public Task<bool> UpdateAsync(Match match);
    public Task<bool> DeleteAsync(int id);
}