using FootballManager.Domain.Entities.Audit;

namespace FootballManager.Domain.Entities
{
    /// <summary>
    ///  Thông tin về tỉ số của một trận đấu giữa hai đội
    /// </summary>
    public class MatchScore : CreModifiedAuditable<int>
    {
        public int MatchId { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }

        /// <summary>
        /// chứa số bàn thắng của đội 1
        /// </summary>
        public short NumberGoalTeam1 { get; set; }

        /// <summary>
        /// chứa số bàn thắng của đội 2
        /// </summary>
        public short NumberGoalTeam2 { get; set; }

        /// <summary>
        /// Lượt đi | lượt về
        /// </summary>
        public string Type { get; set; }
    }
}
