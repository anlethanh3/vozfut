using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Data.DataAccess.Interfaces;
public interface IUnitOfWork
{
    IMemberRepository MemberRepository { get; set; }
}