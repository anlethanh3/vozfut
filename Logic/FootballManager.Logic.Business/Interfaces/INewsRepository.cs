using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface INewsRepository
{
    public Task<News?> GetAsync(int id);
    public Task<IEnumerable<News>> GetAsync();
    public Task<News?> AddAsync(News model);
    public Task<bool> UpdateAsync(News model);
    public Task<bool> DeleteAsync(int id);
    public Task<IEnumerable<News>> SearchAsync(string name);
}