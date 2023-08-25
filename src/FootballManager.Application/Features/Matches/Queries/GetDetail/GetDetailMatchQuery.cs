using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Matches.Queries.GetDetail
{
    public record GetDetailMatchQuery(int Id) : IRequest<Result<GetDetailMatchDto>>;

    internal class GetDetailMatchQueryHandler : IRequestHandler<GetDetailMatchQuery, Result<GetDetailMatchDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly string _connectionString;

        public GetDetailMatchQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetDetailMatchDto>> Handle(GetDetailMatchQuery request, CancellationToken cancellationToken)
        {
            var match = new GetDetailMatchDto();
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var query = $@"SELECT COUNT(1) FROM Matches WHERE Id = @match_id AND IsDeleted = 0";
            var result = await connection.QueryFirstAsync<int>(query, new { match_id = request.Id });
            if (result == 0)
            {
                throw new DomainException("Match not found");
            }

            var queryJoin = $@"SELECT m.Id, m.Name, m.Code, m.TeamSize, m.TeamCount, m.TotalAmount, m.TotalHour, m.FootballFieldSize, m.FootballFieldAddress, m.FootballFieldNumber,
                                  m.MatchDate, m.Description, m.StartTime, m.EndTime, m.Status, m.CreatedDate, m.CreatedBy,
                                  md.Id, md.MemberId, md.MatchId, md.IsPaid, md.IsSkip, md.BibColour, md.CreatedDate, md.CreatedBy,
                                  me.Id, me.Name, me.Description, me.Elo,
                                  v.Id, v.Name, v.Code, v.Status
                           FROM Matches AS m
                            LEFT JOIN MatchDetails AS md ON m.Id = md.MatchId
                            LEFT JOIN Members AS me ON md.MemberId = me.Id
                            LEFT JOIN Votes AS v ON m.VoteId = v.Id
                           WHERE m.Id = @match_id AND m.IsDeleted = 0";

            var data = await connection.QueryAsync<GetDetailMatchDto, MatchDetailDto, GetDetailMatchMemberDto, GetDetailMatchVoteDto, GetDetailMatchDto>(queryJoin,
                (match, matchDetail, member, vote) =>
                {
                    if (member != null)
                    {
                        member.IsPaid = matchDetail.IsPaid;
                        member.IsSkip = matchDetail.IsSkip;
                        member.BibColour = matchDetail.BibColour;
                        match.Members.Add(member);
                    }
                    match.Vote = vote;
                    return match;
                }, new { match_id = request.Id }, splitOn: "Id, Id, Id, Id");

            match = data.GroupBy(x => x.Id).Select(x =>
            {
                match = x.First();
                if (match.Members.Any())
                {
                    match.Members = x.SelectMany(y => y.Members).OrderByDescending(x => x.BibColour).ToList();
                }
                return match;
            }).FirstOrDefault();

            return Result<GetDetailMatchDto>.Success(match);
        }
    }
}
