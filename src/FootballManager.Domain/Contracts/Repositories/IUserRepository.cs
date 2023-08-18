using FootballManager.Domain.Entities;

namespace FootballManager.Domain.Contracts.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<string> GeneratePasswordHashAsync(User user, string password);

        Task<bool> ValidatePasswordAsync(User user, string password);

        Task<bool> CheckExistUserNameAsync(string username);
    }
}
