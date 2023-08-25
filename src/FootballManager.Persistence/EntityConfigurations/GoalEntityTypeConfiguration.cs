using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class GoalEntityTypeConfiguration : IEntityTypeConfiguration<Goals>
    {
        public void Configure(EntityTypeBuilder<Goals> builder)
        {
            builder.ToTable("Goals");
            builder.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn()
                        .HasComment("Primary Key");

            builder.Property(e => e.MatchId).IsRequired();
            builder.Property(e => e.MemberId).IsRequired();
            builder.Property(e => e.MatchScoreId).IsRequired();
            builder.Property(e => e.AssistMemberId);
            builder.Property(e => e.IsOwnGoal);
            builder.Property(e => e.IsPenalty);
            builder.Property(e => e.IsHeader);
            builder.Property(e => e.IsRegularTime);
            builder.Property(e => e.IsExtraTime);
            builder.Property(e => e.IsGoldenGoal);
            builder.Property(e => e.IsDecisiveGoal);
            builder.Property(e => e.Description);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
        }
    }
}
