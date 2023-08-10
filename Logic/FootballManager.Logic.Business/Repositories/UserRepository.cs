using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entity.Entities;
using FootballManager.Logic.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace FootballManager.Logic.Business.Repositories;
public class UserRepository : IUserRepository
{
    private readonly EntityDbContext entityDbContext;

    public UserRepository(EntityDbContext entityDbContext)
    {
        this.entityDbContext = entityDbContext;
    }

    public async Task<User?> AddAsync(User user, string password)
    {
        var record = await GetAsync(user.Email);
        if (record is not null)
        {
            return null;
        }
        var now = DateTime.Now;
        var hashPassword = await GeneratePasswordHashAsync(user, password);
        _ = await entityDbContext.Users.AddAsync(new()
        {
            Name = user.Name,
            Email = user.Email,
            Username = user.Username,
            PasswordHash = hashPassword,
            MemberId = user.MemberId,
            IsAdmin = user.IsAdmin,
            CreatedDate = now,
            ModifiedDate = now,
            IsDeleted = false,
        });
        _ = entityDbContext.SaveChanges();
        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await GetAsync(id);
        if (member is null)
        {
            return false;
        }
        member.IsDeleted = true;
        member.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }
    public async Task<string> GeneratePasswordHashAsync(User user, string password)
    {
        // hash password
        var passwordHasher = new PasswordHasher<User>();
        var hash = passwordHasher.HashPassword(user, $"{password}");
        return hash;
    }

    public async Task<User?> GetAsync(string email)
    {
        return entityDbContext.Users.FirstOrDefault(x => x.Email == email && !x.IsDeleted);
    }

    public async Task<User?> GetAsync(int id)
    {
        return entityDbContext.Users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<User>> GetAsync()
    {
        return entityDbContext.Users.Where(x => x.IsDeleted == false).ToList();
    }

    public async Task<IEnumerable<User>> SearchAsync(string name)
    {
        return entityDbContext.Users.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var record = await GetAsync(user.Id);
        if (record is null)
        {
            return false;
        }
        record.Name = user.Name;
        record.MemberId = user.MemberId;
        record.IsAdmin = user.IsAdmin;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }

    public async Task<bool> ValidatePasswordAsync(User user, string password)
    {
        // validate hash password
        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, $"{password}");
        return result != PasswordVerificationResult.Failed;
    }
}