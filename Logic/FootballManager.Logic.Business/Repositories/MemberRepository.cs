using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entity.Entities;
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
        var elo = CalculateElo(member);
        _ = await entityDbContext.Members.AddAsync(new()
        {
            Name = member.Name,
            Description = member.Description,
            CreatedDate = now,
            ModifiedDate = now,
            Elo = elo,
            Finishing = member.Finishing,
            Passing = member.Passing,
            RealName = member.RealName,
            Skill = member.Skill,
            Speed = member.Speed,
            Stamina = member.Stamina,
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
        return entityDbContext.Members.Where(m => m.Name.ToLower().Contains(name.ToLower()) && m.IsDeleted == false).ToList();
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
        record.RealName = member.RealName;
        record.Finishing = member.Finishing;
        record.Passing = member.Passing;
        record.Skill = member.Skill;
        record.Speed = member.Speed;
        record.Stamina = member.Stamina;
        record.Elo = CalculateElo(member);
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }

    public int CalculateElo(Member member)
    {
        var percents = new[] { 0.8f, 0.8f, 1.15f, 1.15f, 1.1f };
        var stats = new[] { member.Speed, member.Stamina, member.Finishing, member.Passing, member.Skill, };
        var sum = 0f;
        for (int i = 0; i < percents.Length; i++)
        {
            sum += stats[i] * percents[i];
        }
        var average = (int)(sum / percents.Length * 10);
        if (average % 10 > 5)
        {
            return (average / 10) + 1;
        }
        return average / 10;
    }
}