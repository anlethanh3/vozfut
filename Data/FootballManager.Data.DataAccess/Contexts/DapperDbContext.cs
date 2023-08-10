using System.Data;
using FootballManager.Data.DataAccess.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FootballManager.Data.DataAccess.Contexts;

public class DapperDbContext : IDatabaseContext
{
    private readonly IConfiguration configuration;
    private readonly string connectionString;

    public DapperDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
        connectionString = this.configuration.GetConnectionString("SqlConnection") ?? string.Empty;
    }

    public IDbConnection GetDbConnection()
    {
        return new SqlConnection(connectionString);
    }
}