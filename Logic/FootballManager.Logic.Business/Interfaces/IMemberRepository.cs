using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Interfaces;

public interface IMemberRepository
{
    public Task<Member?> GetAsync(int id);
    public Task<IEnumerable<Member>> GetAsync();
    public Task<Member?> AddAsync(Member member);
    public Task<IEnumerable<Member>> SearchAsync(string name);
    public Task<bool> UpdateAsync(Member member);
    public Task<bool> DeleteAsync(int id);
}