using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class MatchScoreEntityTypeConfiguration : IEntityTypeConfiguration<MatchScore>
    {
        public void Configure(EntityTypeBuilder<MatchScore> builder)
        {
            builder.ToTable("MatchScores");
            builder.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn()
                        .HasComment("Primary Key");

            builder.Property(e => e.MatchId).IsRequired();
            builder.Property(e => e.Team1).IsRequired();
            builder.Property(e => e.Team2).IsRequired();
            builder.Property(e => e.NumberGoalTeam1);
            builder.Property(e => e.NumberGoalTeam2);
            builder.Property(e => e.Type);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
        }
    }
}
