using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class MatchEntityTypeConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn()
                    .HasComment("Primary Key");

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.MatchDate).IsRequired();
            builder.Property(e => e.FootballFieldNumber).IsRequired();
            builder.Property(e => e.FootballFieldAddress).IsRequired();
            builder.Property(e => e.FootballFieldSize).IsRequired();
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTime).IsRequired();
            builder.Property(e => e.TotalAmount).IsRequired();
            builder.Property(e => e.TotalHour).IsRequired();
            builder.Property(e => e.Description);
            builder.Property(e => e.TeamSize).IsRequired();
            builder.Property(e => e.TeamCount).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
            builder.Property(e => e.DeletedDate);
            builder.Property(e => e.IsDeleted);
        }
    }
}
