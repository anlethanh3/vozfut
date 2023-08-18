namespace FootballManager.Application.Features.Members.Queries.GetById
{
    public class GetMemberByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short Elo { get; set; }
        public GetMemberPositionDto Position { get; set; }
    }

    public class GetMemberPositionDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
