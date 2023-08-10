using System.Data;
using Microsoft.Extensions.Configuration;

namespace FootballManager.Data.DataAccess.Interfaces;
public interface IDatabaseContext
{
    public IDbConnection GetDbConnection();
}