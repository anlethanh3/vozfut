using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entities;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class MatchDetailRepository : IMatchDetailRepository
{
    private readonly EntityDbContext entityDbContext;

    public MatchDetailRepository(EntityDbContext entityDbContext)
    {
        this.entityDbContext = entityDbContext;
    }

    public async Task<MatchDetail?> AddAsync(MatchDetail detail)
    {
        var record = await GetAsync(detail.MatchId, detail.MemberId);
        if (record is not null)
        {
            return null;
        }
        var now = DateTime.Now;
        _ = await entityDbContext.MatchDetails.AddAsync(new()
        {
            MatchId = detail.MatchId,
            MemberId = detail.MemberId,
            IsPaid = detail.IsPaid,
            IsSkip = detail.IsSkip,
            CreatedDate = now,
            ModifiedDate = now,
            IsDeleted = false,
        });
        _ = entityDbContext.SaveChanges();
        return detail;
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

    public async Task<MatchDetail?> GetAsync(int id)
    {
        return entityDbContext.MatchDetails.FirstOrDefault(m => m.Id == id);
    }

    public async Task<MatchDetail?> GetAsync(int matchId, int memberId)
    {
        return entityDbContext.MatchDetails.FirstOrDefault(x => x.MatchId == matchId && x.MemberId == memberId && !x.IsDeleted);
    }

    public async Task<IEnumerable<MatchDetail>> GetAsync()
    {
        return entityDbContext.MatchDetails.Where(x => x.IsDeleted == false).ToList();
    }

    public async Task<bool> UpdateAsync(MatchDetail detail)
    {
        var record = await GetAsync(detail.Id);
        if (record is null)
        {
            return false;
        }
        record.MatchId = detail.MatchId;
        record.MemberId = detail.MemberId;
        record.IsPaid = detail.IsPaid;
        record.IsSkip = detail.IsSkip;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }
}