using FootballManager.Domain.Entities;
using FootballManager.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Context
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
        }
    }
}
