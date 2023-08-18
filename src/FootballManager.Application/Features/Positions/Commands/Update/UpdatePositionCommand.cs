using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Positions.Commands.Update
{
    public record UpdatePositionCommand : RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    internal class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Result<bool>>
    {
        private readonly IAsyncRepository<Position> _positionRepository;

        public UpdatePositionCommandHandler(IAsyncRepository<Position> positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<bool>> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = await _positionRepository.GetAsync(request.Id) ?? throw new DomainException("Position not found");

            position.Name = request.Name;
            position.Code = request.Code;
            position.Description = request.Description;
            position.ModifiedDate = request.RequestedAt;
            position.ModifiedBy = request.RequestedBy;

            var updated = await _positionRepository.UpdateAsync(position);

            return updated != null ? Result<bool>.Success(true) : Result<bool>.Failure();
        }
    }
}
