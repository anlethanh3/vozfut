using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Commands.Create
{
    public record CreateMatchCommand : RequestAudit, IRequest<Result<int>>
    {
        public int? VoteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short TeamSize { get; set; }
        public short TeamCount { get; set; }
        public DateTime MatchDate { get; set; }
        public double TotalAmount { get; set; }
        public double TotalHour { get; set; }
        public short FootballFieldSize { get; set; }
        public string FootballFieldAddress { get; set; }
        public short FootballFieldNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
