namespace FootballManager.Application.Features.MatchDetails.Queries.GetTeam
{
    public class GetTeamByMatchDto
    {
        public GetTeamByMatchDto()
        {
            Teams = new List<BibColourTeamDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<BibColourTeamDto> Teams { get; set; }
    }

    public class BibColourTeamDto
    {
        public int Id { get; set; }
        public string BibColour { get; set; }
    }
    public class MatchDto
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public short TeamSize { get; set; }
        public short TeamCount { get; set; }
    }
}
