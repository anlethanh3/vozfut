using Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Vote : FullAuditable<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public IList<MemberVote> MemberVotes { get; set; }
    }
}
