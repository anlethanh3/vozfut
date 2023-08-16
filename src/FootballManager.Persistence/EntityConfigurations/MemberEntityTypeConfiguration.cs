﻿using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");

            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn()
                   .HasComment("Primary Key");

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Description);
            builder.Property(e => e.Elo).IsRequired();
            builder.Property(e => e.PositionId);
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.ModifiedBy);
            builder.Property(e => e.ModifiedDate);
            builder.Property(e => e.DeletedDate);
            builder.Property(e => e.IsDeleted);
        }
    }
}
