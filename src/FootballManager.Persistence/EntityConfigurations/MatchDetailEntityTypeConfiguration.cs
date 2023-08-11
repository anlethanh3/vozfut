using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class MatchDetailEntityTypeConfiguration : IEntityTypeConfiguration<MatchDetail>
    {
        public void Configure(EntityTypeBuilder<MatchDetail> builder)
        {
            builder.ToTable("MatchDetails");

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn()
                    .HasComment("Primary Key");

            builder.Property(e => e.MatchId).IsRequired();
            builder.Property(e => e.MemberId);
            builder.Property(e => e.IsPaid);
            builder.Property(e => e.IsSkip);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
            builder.Property(e => e.DeletedDate);
            builder.Property(e => e.IsDeleted);
        }
    }
}
