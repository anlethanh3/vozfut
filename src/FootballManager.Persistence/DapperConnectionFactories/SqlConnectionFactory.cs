using System.Data;
using FootballManager.Domain.Contracts.DapperConnection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FootballManager.Persistence.DapperConnectionFactories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly ILogger<SqlConnectionFactory> _logger;
        private IDbConnection _connection;

        public SqlConnectionFactory(ILogger<SqlConnectionFactory> logger)
        {
            _logger = logger;
        }

        public async Task<IDbConnection> CreateConnectionAsync(string connectionString)
        {
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    _connection = new SqlConnection(connectionString);
                    _connection.Open();
                }

                return _connection;
            }
            catch (Exception)
            {
                _logger.LogError("cannot connect to database with connection string: {ConnectionString}", connectionString);
                throw;
            }
        }
    }
}
