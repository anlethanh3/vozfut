namespace FootballManager.Application.Features.MatchDetails.Queries.Rolling
{
    public class RollingTeamMatchDetailDto
    {
        public short TotalElo { get; set; }
        public string BibColour { get; set; }
        public List<RollingTeamMatchDetailMemberDto> Members { get; set; }
    }

    public class RollingTeamMatchDetailMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Elo { get; set; }
    }

    public class MemberVoteDto
    {
        public int MemberId { get; set; }
        public int VoteId { get; set; }
        public DateTime VoteDate { get; set; }
        public bool IsJoin { get; set; }
    }

    public class MatchDto
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public short TeamSize { get; set; }
        public short TeamCount { get; set; }
    }
}
