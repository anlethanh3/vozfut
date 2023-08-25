using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Application.Features.Members.Commands.Update
{
    internal class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Position> _positionRepository;
        private readonly IAsyncRepository<Position> _subPositionRepository;

        public UpdateMemberCommandHandler(IAsyncRepository<Member> memberRepository,
            IAsyncRepository<Position> positionRepository,
            IAsyncRepository<Position> subPositionRepository)
        {
            _memberRepository = memberRepository;
            _positionRepository = positionRepository;
            _subPositionRepository = subPositionRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.Entities.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new DomainException("Member not found");

            member.Name = request.Name;
            member.Description = request.Description;
            member.Elo = (short)request.Elo;
            member.PositionId = request.PositionId;
            member.SubPositionId = request.SubPositionId;
            member.ModifiedDate = request.RequestedAt;
            member.ModifiedBy = request.RequestedBy;

            if (request.PositionId.HasValue)
            {
                _ = await _positionRepository.GetAsync(x => x.Id == request.PositionId.Value) ?? throw new DomainException("Position not found");
                member.PositionId = request.PositionId.Value;

                _ = await _subPositionRepository.GetAsync(x => x.Id == request.SubPositionId.Value) ?? throw new DomainException("Sub-Position not found");
                member.SubPositionId = request.SubPositionId;
            }

            var updated = await _memberRepository.UpdateAsync(member);

            return updated != null ? await Result<bool>.SuccessAsync(true) : await Result<bool>.FailureAsync(false);
        }
    }
}
