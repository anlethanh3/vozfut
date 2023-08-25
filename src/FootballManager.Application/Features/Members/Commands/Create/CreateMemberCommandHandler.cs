using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Members.Commands.Create
{
    internal class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<int>>
    {
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Position> _positionRepository;
        private readonly IAsyncRepository<Position> _subPositionRepository;

        public CreateMemberCommandHandler(IAsyncRepository<Member> memberRepository,
            IAsyncRepository<Position> positionRepository,
            IAsyncRepository<Position> subPositionRepository)
        {
            _memberRepository = memberRepository;
            _positionRepository = positionRepository;
            _subPositionRepository = subPositionRepository;
        }

        public async Task<Result<int>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = new Member
            {
                Name = request.Name,
                Description = request.Description,
                Elo = (short)request.Elo,
                SubPositionId = request.SubPositionId,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            if (request.PositionId.HasValue)
            {
                _ = await _positionRepository.GetAsync(x => x.Id == request.PositionId.Value) ?? throw new DomainException("Position not found");
                member.PositionId = request.PositionId.Value;

                _ = await _subPositionRepository.GetAsync(x => x.Id == request.SubPositionId.Value) ?? throw new DomainException("Sub-Position not found");
                member.SubPositionId = request.SubPositionId;
            }

            var created = await _memberRepository.CreateAsync(member);
            return created != null ? await Result<int>.SuccessAsync(1) : await Result<int>.FailureAsync();
        }
    }
}
