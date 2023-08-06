using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Data.DataAccess.Repositories;
public class UnitOfWork : IUnitOfWork
{
    public required IMemberRepository MemberRepository { get; set; }

    public UnitOfWork(IMemberRepository memberRepository)
    {
        MemberRepository = memberRepository;
    }
}