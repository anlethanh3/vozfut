using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn()
                   .HasComment("Primary Key");

            builder.Property(e => e.Username).IsRequired();
            builder.Property(e => e.PasswordHash).IsRequired();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Email);
            builder.Property(e => e.MemberId);
            builder.Property(e => e.IsAdmin);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
            builder.Property(e => e.DeletedDate);
            builder.Property(e => e.IsDeleted);
        }
    }
}
