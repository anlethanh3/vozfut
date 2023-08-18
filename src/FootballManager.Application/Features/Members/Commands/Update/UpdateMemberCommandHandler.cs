using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Commands.Update
{
    internal class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Member> _memberRepository;

        public UpdateMemberCommandHandler(IAsyncRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetAsync(request.Id) ?? throw new DomainException("Member not found");

            member.Name = request.Name;
            member.Description = request.Description;
            member.Elo = (short)request.Elo;
            member.PositionId = request.PositionId;
            member.ModifiedDate = request.RequestedAt;
            member.ModifiedBy = request.RequestedBy;

            var updated = await _memberRepository.UpdateAsync(member);

            return updated != null ? await Result<bool>.SuccessAsync(true) : await Result<bool>.FailureAsync(false);
        }
    }
}
