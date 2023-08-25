using Dapper;
using FootballManager.Domain.Contracts.DapperConnection;
using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.OptionsSettings;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.MemberVotes.Commands.Create
{
    public class CreateOrUpdateMemberVoteCommandHandler : IRequestHandler<CreateOrUpdateMemberVoteCommand, Result<int>>
    {
        private readonly IAsyncRepository<MemberVote> _memberVoteRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Vote> _voteRepository;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _connectionString;

        public CreateOrUpdateMemberVoteCommandHandler(IAsyncRepository<MemberVote> memberVoteRepository,
            IAsyncRepository<Member> memberRepository,
            IAsyncRepository<Vote> voteRepository,
            ISqlConnectionFactory sqlConnectionFactory,
            ConnectionOptions connectionOptions)
        {
            _memberVoteRepository = memberVoteRepository;
            _memberRepository = memberRepository;
            _voteRepository = voteRepository;
            _sqlConnectionFactory = sqlConnectionFactory;
            _connectionString = connectionOptions.SqlConnectionString;
        }

        public async Task<Result<int>> Handle(CreateOrUpdateMemberVoteCommand request, CancellationToken cancellationToken)
        {
            var member = _memberRepository.Entities.Where(x => x.Id == request.MemberId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Member not found");

            var vote = _voteRepository.Entities.Where(x => x.Id == request.VoteId && !x.IsDeleted).FirstOrDefault() ?? throw new DomainException("Vote not found");

            //Nếu member đã tồn tại trong vote rồi thì update theo param truyền vào, ngược lại thì tạo mới
            var memberVote = new MemberVote
            {
                MemberId = member.Id,
                VoteId = vote.Id,
                IsJoin = request.IsJoin
            };
            var checkAlreadyExits = _memberVoteRepository.Entities.Where(x => x.VoteId == request.VoteId && x.MemberId == request.MemberId).FirstOrDefault();
            if (checkAlreadyExits != null)
            {
                using var connection = await _sqlConnectionFactory.CreateConnectionAsync(_connectionString);
                var sql = $@"UPDATE MemberVotes
                             SET IsJoin = @is_join
                             WHERE Memberid = @member_id AND VoteId = @vote_id";
                var executed = await connection.ExecuteAsync(sql, new
                {
                    is_join = memberVote.IsJoin,
                    member_id = memberVote.MemberId,
                    vote_id = memberVote.VoteId
                });

                return executed > 0 ? Result<int>.Success(1) : Result<int>.Failure();
            }
            else
            {
                memberVote.VoteDate = request.RequestedAt;
                var created = await _memberVoteRepository.CreateAsync(memberVote);

                return created != null ? Result<int>.Success(1) : Result<int>.Failure();
            }
        }
    }
}
