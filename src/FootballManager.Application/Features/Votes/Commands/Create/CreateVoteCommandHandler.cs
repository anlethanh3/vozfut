using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using FootballManager.Domain.ResultModels;
using FootballManager.Infrastructure.Helpers;
using MediatR;

namespace FootballManager.Application.Features.Votes.Commands.Create
{
    internal class CreateVoteCommandHandler : IRequestHandler<CreateVoteCommand, Result<int>>
    {
        private readonly IAsyncRepository<Vote> _voteRepository;

        public CreateVoteCommandHandler(IAsyncRepository<Vote> voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<Result<int>> Handle(CreateVoteCommand request, CancellationToken cancellationToken)
        {
            var vote = new Vote
            {
                Name = request.Name,
                Code = RandomHelper.RandomString(5),
                Status = VoteStatusEnum.Open.Name,
                Description = request.Description,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            var created = await _voteRepository.CreateAsync(vote);

            return created != null ? Result<int>.Success(1) : Result<int>.Failure();
        }
    }
}
