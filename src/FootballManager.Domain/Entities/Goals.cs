using FootballManager.Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    public class Goals : CreModifiedAuditable<int>
    {
        public int MatchId { get; set; }
        public int MatchScoreId { get; set; }
        public int MemberId { get; set; }
        public int? AssistMemberId { get; set; }
        public DateTime Minute { get; set; }
        public bool IsOwnGoal { get; set; }
        public bool IsPenalty { get; set; }
        public bool IsHeader { get; set; }
        public bool IsRegularTime { get; set; }
        public bool IsExtraTime { get; set; }
        public bool IsGoldenGoal { get; set; }
        public bool IsDecisiveGoal { get; set; }
        public string Description { get; set; }
    }
}
