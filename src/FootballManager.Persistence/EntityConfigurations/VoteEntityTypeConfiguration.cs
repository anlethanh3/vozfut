using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class VoteEntityTypeConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Votes");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .HasComment("Primary Key");
            builder.Property("Name").IsRequired();
            builder.Property("Code");
            builder.Property("Description");
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
            builder.Property(e => e.DeletedDate);
            builder.Property(e => e.IsDeleted);
        }
    }
}
