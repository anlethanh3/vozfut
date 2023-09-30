using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Data.DataAccess.Interfaces;
public interface IUnitOfWork
{
    IMemberRepository MemberRepository { get; set; }
    IMatchRepository MatchRepository { get; set; }
    IMatchDetailRepository MatchDetailRepository { get; set; }
    IUserRepository UserRepository { get; set; }
    ITeamRivalRepository TeamRivalRepository { get; set; }
    INewsRepository NewsRepository { get; set; }
}