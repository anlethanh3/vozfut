namespace FootballManager.Application.Features.Votes.Queries.GetById
{
    public class GetVoteByIdDto
    {
        public GetVoteByIdDto()
        {
            Members = new List<GetVoteByIdMemberDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public virtual List<GetVoteByIdMemberDto> Members { get; set; }
    }

    public class GetVoteByIdMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsJoin { get; set; }
        public DateTime VoteDate { get; set; }
    }

    public class MemberVoteDto
    {
        public int MemberId { get; set; }
        public int VoteId { get; set; }
        public bool IsJoin { get; set; }
        public DateTime VoteDate { get; set; }
    }
}
