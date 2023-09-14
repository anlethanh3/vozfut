using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Responses;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class MatchDetailRepository : IMatchDetailRepository
{
    private readonly EntityDbContext entityDbContext;
    private readonly IMatchDetailContext matchDetailContext;

    public MatchDetailRepository(EntityDbContext entityDbContext, IMatchDetailContext matchDetailContext)
    {
        this.entityDbContext = entityDbContext;
        this.matchDetailContext = matchDetailContext;
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

    public async Task<IEnumerable<MatchDetailResponse>> GetAllAsync(int id)
    {
        var match = entityDbContext.Matches.Where(x => x.Id == id).FirstOrDefault();
        if (match is null)
        {
            return new List<MatchDetailResponse>();
        }
        var members = entityDbContext.Members.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
        var details = entityDbContext.MatchDetails.Where(x => !x.IsDeleted && x.MatchId == id).ToList();

        Func<Member, MatchDetailResponse> func = member =>
        {
            var now = DateTime.MinValue;
            var emptyDetail = new MatchDetailResponse
            {
                Id = 0,
                IsPaid = false,
                IsSkip = false,
                CreatedDate = now,
                ModifiedDate = now,
                IsDeleted = false,
                MatchId = match.Id,
                MatchName = match.Name,
                MemberElo = member.Elo,
                MemberId = member.Id,
                MemberName = member.Name,
            };

            if (details.Count() == 0)
            {
                return emptyDetail;
            }

            var detail = details.FirstOrDefault(x => x.MemberId == member.Id && x.MatchId == id);
            if (detail is null)
            {
                return emptyDetail;
            }

            return new MatchDetailResponse
            {
                Id = detail.Id,
                IsPaid = detail.IsPaid,
                IsSkip = detail.IsSkip,
                CreatedDate = detail.CreatedDate,
                ModifiedDate = detail.ModifiedDate,
                IsDeleted = detail.IsDeleted,
                MatchId = match.Id,
                MatchName = match.Name,
                MemberElo = member.Elo,
                MemberId = member.Id,
                MemberName = member.Name,
            };
        };

        var result = members.Select(member => func(member)).OrderByDescending(c=>c.ModifiedDate);
        return result;
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

    public async Task<MatchDetail> UpdateNewAsync(MatchDetail detail)
    {
        var now = DateTime.Now;
        var record = await GetAsync(detail.MatchId, detail.MemberId);

        if (record is null)
        {
            var result = await entityDbContext.MatchDetails.AddAsync(new()
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
            return result.Entity;
        }

        record.MatchId = detail.MatchId;
        record.MemberId = detail.MemberId;
        record.IsPaid = detail.IsPaid;
        record.IsSkip = detail.IsSkip;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return record;
    }

    public async Task<IEnumerable<Member>> GetMembersAsync(int id)
    {
        var match = entityDbContext.Matches.Where(x => x.Id == id).FirstOrDefault();
        if (match is null)
        {
            return new List<Member>();
        }
        var members = entityDbContext.Members.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
        var details = entityDbContext.MatchDetails.Where(x => !x.IsDeleted && x.MatchId == id).ToList();

        var result = members.Where(member => details.Any(x=>x.MemberId==member.Id)).OrderBy(c=>c.Name);
        return result;
    }
}