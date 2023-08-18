using System.Data;

namespace FootballManager.Domain.Contracts.DapperConnection
{
    public interface ISqlConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(string connectionString);
    }
}
