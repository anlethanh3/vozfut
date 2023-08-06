using Microsoft.EntityFrameworkCore;
using FootballManager.Data.Entities;
using Microsoft.Extensions.Configuration;
using FootballManager.Data.DataAccess.Interfaces;

namespace FootballManager.Data.DataAccess.Contexts;

public class EntityDbContext : DbContext
{
    private IConfiguration Configuration { get; set; }
    private string ConnectionString { get; set; }
    public DbSet<Member> Members { get; set; }

    public EntityDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
        ConnectionString = Configuration.GetConnectionString("SqlConnection") ?? string.Empty;
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(ConnectionString);
}