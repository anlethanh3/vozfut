using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Data.DataAccess.Repositories;
public class UnitOfWork : IUnitOfWork
{
    public required IMemberRepository MemberRepository { get; set; }
    public required IMatchRepository MatchRepository { get; set; }
    public required IMatchDetailRepository MatchDetailRepository { get; set; }
    public required IUserRepository UserRepository { get; set; }
    public required ITeamRivalRepository TeamRivalRepository { get; set; }
    public required INewsRepository NewsRepository { get; set; }

    public UnitOfWork(
        IMemberRepository memberRepository,
        IMatchRepository matchRepository,
        IMatchDetailRepository matchDetailRepository,
        IUserRepository userRepository,
        ITeamRivalRepository teamRivalRepository,
        INewsRepository newsRepository)
    {
        MemberRepository = memberRepository;
        MatchRepository = matchRepository;
        MatchDetailRepository = matchDetailRepository;
        UserRepository = userRepository;
        TeamRivalRepository = teamRivalRepository;
        NewsRepository = newsRepository;
    }
}