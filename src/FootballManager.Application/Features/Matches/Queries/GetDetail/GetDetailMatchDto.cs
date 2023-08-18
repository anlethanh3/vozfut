namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public class GetDetailMatchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public short TeamSize { get; set; }
        public short TeamCount { get; set; }
        public double TotalHour { get; set; }
        public short SoccerFieldSize { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
