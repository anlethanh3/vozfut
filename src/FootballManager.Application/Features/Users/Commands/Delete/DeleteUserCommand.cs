using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Commands.Delete
{
    public record DeleteUserCommand: RequestAudit, IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id) ?? throw new DomainException("ERR_USER_NOT_EXIST");

            user.IsDeleted = true;
            user.DeletedDate = request.RequestedAt;

            var updated = await _userRepository.UpdateAsync(user);

            return await Result<bool>.SuccessAsync(updated != null);
        }
    }
}
