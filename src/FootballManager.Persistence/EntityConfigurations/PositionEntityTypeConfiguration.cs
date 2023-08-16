using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class PositionEntityTypeConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn()
                        .HasComment("Primary Key");

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Code).IsRequired();
            builder.Property(e => e.Descrption);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
        }
    }
}
