using FootballManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballManager.Persistence.EntityConfigurations
{
    public class MemberVoteEntityTypeConfiguration : IEntityTypeConfiguration<MemberVote>
    {
        public void Configure(EntityTypeBuilder<MemberVote> builder)
        {
            builder.HasKey(mv => new { mv.MemberId, mv.VoteId });

            builder.HasOne<Member>(mv => mv.Member)
                   .WithMany(m => m.MemberVotes)
                   .HasForeignKey(mv => mv.MemberId);

            builder.HasOne<Vote>(mv => mv.Vote)
                   .WithMany(m => m.MemberVotes)
                   .HasForeignKey(mv => mv.VoteId);
        }
    }
}
