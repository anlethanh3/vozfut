using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Commands.Create
{
    internal class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<int>>
    {
        private readonly IAsyncRepository<Member> _memberRepository;

        public CreateMemberCommandHandler(IAsyncRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Result<int>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = new Member
            {
                Name = request.Name,
                Description = request.Description,
                Elo = (short)request.Elo,
                PositionId = request.PositionId,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var created = await _memberRepository.CreateAsync(member);
            return created != null ? await Result<int>.SuccessAsync(1) : await Result<int>.FailureAsync();
        }
    }
}
