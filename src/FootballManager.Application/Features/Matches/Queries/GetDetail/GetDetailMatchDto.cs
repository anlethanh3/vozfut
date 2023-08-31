namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public class GetDetailMatchDto
    {
        public GetDetailMatchDto()
        {
            Teams = new List<TeamOfMatchDto>();
            Vote = new GetDetailMatchVoteDto();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public short TeamSize { get; set; }
        public short TeamCount { get; set; }
        public double TotalHour { get; set; }
        public short FootballFieldSize { get; set; }
        public string FootballFieldAddress { get; set; }
        public short FootballFieldNumber { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public GetDetailMatchVoteDto Vote { get; set; }

        public List<TeamOfMatchDto> Teams { get; set; }
    }

    public class GetDetailMatchVoteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
    }

    public class GetDetailMatchMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsPaid { get; set; }
        public bool IsSkip { get; set; }
        public string BibColour { get; set; }
    }

    public class TeamOfMatchDto
    {
        public string Name { get; set; }

        public List<MemberOfTeamMatchDto> Members { get; set; }
    }

    public class MemberOfTeamMatchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSkip { get; set; }
    }

    public class MatchDetailDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int MatchId { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSkip { get; set; }
        public string BibColour { get; set; }
    }
}
