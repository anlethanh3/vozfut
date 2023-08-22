using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Responses;

namespace FootballManager.Logic.Business.Interfaces;

public interface IMatchDetailRepository
{
    public Task<MatchDetail?> GetAsync(int id);
    public Task<MatchDetail> GetAsync(int matchId,int memberId);
    public Task<IEnumerable<MatchDetail>> GetAsync();
    public Task<MatchDetail?> AddAsync(MatchDetail detail);
    public Task<bool> UpdateAsync(MatchDetail detail);
    public Task<bool> DeleteAsync(int id);
    public Task<IEnumerable<MatchDetailResponse>> GetAllAsync(int matchId);
}