using System.Data;

namespace FootballManager.Data.DataAccess.Interfaces;
public interface IDatabaseContext
{
    public IDbConnection GetDbConnection();
}