using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entities;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class MemberRepository : IMemberRepository
{
    private readonly EntityDbContext entityDbContext;

    public MemberRepository(EntityDbContext entityDbContext)
    {
        this.entityDbContext = entityDbContext;
    }

    public async Task<Member?> AddAsync(Member member)
    {
        var now = DateTime.Now;
        _ = await entityDbContext.Members.AddAsync(new()
        {
            Name = member.Name,
            Description = member.Description,
            CreatedDate = now,
            ModifiedDate = now,
            Elo = member.Elo,
            IsDeleted = false,
        });
        _ = entityDbContext.SaveChanges();
        return member;
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

    public async Task<Member?> GetAsync(int id)
    {
        return entityDbContext.Members.FirstOrDefault(m => m.Id == id);
    }

    public async Task<IEnumerable<Member>> GetAsync()
    {
        return entityDbContext.Members.Where(x => x.IsDeleted == false).ToList();
    }

    public async Task<IEnumerable<Member>> SearchAsync(string name)
    {
        return entityDbContext.Members.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task<bool> UpdateAsync(Member member)
    {
        var record = await GetAsync(member.Id);
        if (record is null)
        {
            return false;
        }
        record.Name = member.Name;
        record.Description = member.Description;
        record.Elo = member.Elo;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }
}