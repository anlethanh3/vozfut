using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Features.Members.Commands.Delete
{
    public record DeleteMemberCommand(int Id) : RequestAudit, IRequest<Result<bool>>;

    internal class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Member> _memberRepository;

        public DeleteMemberCommandHandler(IAsyncRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Result<bool>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new DomainException("Member not found");

            member.DeletedDate = request.RequestedAt;
            member.IsDeleted = true;

            var deleted = await _memberRepository.UpdateAsync(member);

            return deleted != null ? await Result<bool>.SuccessAsync(true) : await Result<bool>.FailureAsync(false);
        }
    }
}
