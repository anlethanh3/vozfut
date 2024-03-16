using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entity.Entities;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class MatchRepository : IMatchRepository
{
    private readonly EntityDbContext entityDbContext;

    public MatchRepository(EntityDbContext entityDbContext)
    {
        this.entityDbContext = entityDbContext;
    }

    public async Task<Match?> AddAsync(Match match)
    {
        var now = DateTime.Now;
        _ = await entityDbContext.Matches.AddAsync(new()
        {
            Name = match.Name,
            Description = match.Description,
            CreatedDate = now,
            ModifiedDate = now,
            TeamCount = match.TeamCount,
            TeamSize = match.TeamSize,
            IsDeleted = false,
        });
        _ = entityDbContext.SaveChanges();
        return match;
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

    public async Task<Match?> GetAsync(int id)
    {
        return entityDbContext.Matches.FirstOrDefault(m => m.Id == id);
    }

    public async Task<IEnumerable<Match>> GetAsync()
    {
        return entityDbContext.Matches.Where(x => x.IsDeleted == false).ToList();
    }

    public async Task<IEnumerable<Match>> SearchAsync(string name)
    {
        return entityDbContext.Matches.Where(m => m.Name.ToLower().Contains(name.ToLower()) && !m.IsDeleted).OrderByDescending(x => x.ModifiedDate).ToList();
    }

    public async Task<bool> UpdateAsync(Match match)
    {
        var record = await GetAsync(match.Id);
        if (record is null)
        {
            return false;
        }
        record.Name = match.Name;
        record.Description = match.Description;
        record.TeamSize = match.TeamSize;
        record.TeamCount = match.TeamCount;
        record.TeamRivals = match.TeamRivals;
        record.HasTeamRival = match.HasTeamRival;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }
}