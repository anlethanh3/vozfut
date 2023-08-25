using System.Data;
using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Votes.Queries.GetById
{
    public record GetVoteByIdQuery(int Id) : IRequest<Result<GetVoteByIdDto>>;

    internal class GetVoteByIdQueryHandler : IRequestHandler<GetVoteByIdQuery, Result<GetVoteByIdDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public GetVoteByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<GetVoteByIdDto>> Handle(GetVoteByIdQuery request, CancellationToken cancellationToken)
        {
            var vote = new GetVoteByIdDto();
            using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);

            var checkExist = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(1) FROM Votes WHERE Id = @id AND IsDeleted = 0", new { id = request.Id });
            if (checkExist == 0)
            {
                throw new DomainException("Vote not found");
            }

            var query = $@"SELECT v.Id, v.Name, v.Code, v.Description, v.CreatedDate, v.CreatedBy,
                                  mv.MemberId, mv.VoteId, mv.IsJoin, mv.VoteDate,
                                  m.Id, m.Name
                           FROM Votes AS v
                             LEFT JOIN MemberVotes AS mv ON v.Id = mv.VoteId
                             LEFT JOIN Members AS m ON mv.MemberId = m.Id
                           WHERE v.IsDeleted = 0 AND v.Id = @id";

            var data = await connection.QueryAsync<GetVoteByIdDto, MemberVoteDto, GetVoteByIdMemberDto, GetVoteByIdDto>(query, (vote, memberVote, member) =>
            {
                if (member != null)
                {
                    member.IsJoin = memberVote.IsJoin;
                    member.VoteDate = memberVote.VoteDate;
                    vote.Members.Add(member);
                }
                return vote;
            }, new { id = request.Id }, splitOn: "Id, MemberId, Id");

            vote = data.GroupBy(x => x.Id).Select(x =>
            {
                vote = x.First();
                if (vote.Members.Any())
                {
                    vote.Members = x.SelectMany(y => y.Members).OrderBy(x => x.VoteDate).ToList();
                }
                return vote;
            }).FirstOrDefault();

            return Result<GetVoteByIdDto>.Success(vote);
        }
    }
}
