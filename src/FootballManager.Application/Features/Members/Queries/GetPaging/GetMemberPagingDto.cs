namespace FootballManager.Application.Features.Members.Queries.GetPaging
{
    public class GetMemberPagingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Elo { get; set; }
        public string Description { get; set; }
        public GetMemberPagingPositionDto Position { get; set; }
        public GetMemberPagingSubPositionDto SubPosition { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GetMemberPagingPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class GetMemberPagingSubPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
