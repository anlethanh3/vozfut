using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Positions.Commands.Create
{
    public record CreatePositionCommand : RequestAudit, IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    internal class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Result<int>>
    {
        private readonly IAsyncRepository<Position> _positionRepository;

        public CreatePositionCommandHandler(IAsyncRepository<Position> positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<int>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = new Position
            {
                Name = request.Name,
                Description = request.Description,
                Code = request.Code,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var created = await _positionRepository.CreateAsync(position);

            return created != null ? Result<int>.Success(1) : Result<int>.Failure();
        }
    }
}
