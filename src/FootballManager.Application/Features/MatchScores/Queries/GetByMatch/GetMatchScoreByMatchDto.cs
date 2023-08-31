namespace FootballManager.Application.Features.MatchScores.Queries.GetByMatch
{
    public class GetMatchScoreByMatchDto
    {
        public GetMatchScoreByMatchDto()
        {
            Schedules = new List<MatchScheduleDto>();
        }

        public int MatchId { get; set; }
        public List<MatchScheduleDto> Schedules { get; set; }
    }

    public class MatchScheduleDto
    {
        public int Id { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public short NumberGoalTeam1 { get; set; }
        public short NumberGoalTeam2 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
