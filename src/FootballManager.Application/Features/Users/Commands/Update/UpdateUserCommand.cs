using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Commands.Update
{
    public record UpdateUserCommand(int Id, string Name, int? MemberId, bool IsAdmin) : RequestAudit, IRequest<Result<bool>>;

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.Id) ?? throw new DomainException("ERR_USER_NOT_EXIST");

            user.Name = request.Name;
            user.MemberId = request.MemberId;
            user.IsAdmin = request.IsAdmin;
            user.ModifiedBy = request.RequestedBy;
            user.ModifiedDate = request.RequestedAt;

            var updated = await _userRepository.UpdateAsync(user);

            return await Result<bool>.SuccessAsync(updated != null);
        }
    }
}
