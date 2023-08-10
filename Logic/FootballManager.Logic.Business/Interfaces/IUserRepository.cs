using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetAsync(int id);
    public Task<IEnumerable<User>> GetAsync();
    public Task<User?> GetAsync(string email);
    public Task<User?> AddAsync(User user);
    public Task<IEnumerable<User>> SearchAsync(string name);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> DeleteAsync(int id);
    public Task<string> GeneratePasswordHashAsync(User user);
    public Task<bool> ValidatePasswordAsync(User user, string password);
}