using Microsoft.EntityFrameworkCore;
using FootballManager.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace FootballManager.Data.DataAccess.Contexts;

public class EntityDbContext : DbContext
{
    private IConfiguration Configuration { get; set; }
    private string ConnectionString { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchDetail> MatchDetails { get; set; }

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