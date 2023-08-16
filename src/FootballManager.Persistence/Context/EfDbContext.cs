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
        public DbSet<Member> Members { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<MemberVote> MemberVotes { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchDetail> MatchDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberVoteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MatchDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MatchDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionEntityTypeConfiguration());
        }
    }
}
