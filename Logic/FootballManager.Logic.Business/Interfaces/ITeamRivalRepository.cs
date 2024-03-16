using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;

namespace FootballManager.Logic.Business.Interfaces;

public interface ITeamRivalRepository
{
    public Task<IEnumerable<TeamRival>> CreateAsync(int id);
    public Task<IEnumerable<TeamRival>> GetAsync(int id);
    public Task<TeamRivalSchedule> GetMatchScheduleAsync(int id);
    public Task<bool> ExchangeMemberAsync(ExchangeMemberRequest model);
    public Task<bool> MemberInOutAsync(MemberInOutRequest model);
    public Task<bool> UpdateWinnerAsync(WinnerUpdateRequest model);
    public Task<bool> SaveAsync(int matchId, IEnumerable<TeamRival> teams);
}