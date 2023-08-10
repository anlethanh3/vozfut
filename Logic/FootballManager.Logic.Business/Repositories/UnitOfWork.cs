using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Data.DataAccess.Repositories;
public class UnitOfWork : IUnitOfWork
{
    public required IMemberRepository MemberRepository { get; set; }
    public IMatchRepository MatchRepository { get; set; }
    public IMatchDetailRepository MatchDetailRepository { get; set; }

    public UnitOfWork(
        IMemberRepository memberRepository,
        IMatchRepository matchRepository,
        IMatchDetailRepository matchDetailRepository)
    {
        MemberRepository = memberRepository;
        MatchRepository = matchRepository;
        MatchDetailRepository = matchDetailRepository;
    }
}