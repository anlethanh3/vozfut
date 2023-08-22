using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Responses;

namespace FootballManager.Data.DataAccess.Interfaces;

public interface IMatchDetailContext{
    public Task<IEnumerable<MatchDetailResponse>> GetAllAsync(int matchId);
    public Task<bool> UpdateAsync(MatchDetail detail);
}