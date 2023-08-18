using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Member : FullAuditable<int>
    {
        public string Name { get; set; }
        public short Elo { get; set; }
        public int? PositionId { get; set; }

        public int? SubPositionId { get; set; }
        public string Description { get; set; }
        public IList<MemberVote> MemberVotes { get; set; }
    }
}
