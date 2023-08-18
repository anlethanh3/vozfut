using FootballManager.Domain.Contracts.Repositories;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Exceptions;
using FootballManager.Domain.ResultModels;
using MediatR;

namespace FootballManager.Application.Features.Users.Commands.Create
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.CheckExistUserNameAsync(request.Username))
            {
                throw new DomainException("Username already exists");
            }

            var passwordHash = await _userRepository.GeneratePasswordHashAsync(null, request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                IsAdmin = request.IsAdmin,
                MemberId = request.MemberId,
                CreatedBy = request.RequestedBy,
                CreatedDate = request.RequestedAt
            };

            await _userRepository.CreateAsync(user);

            return await Result<int>.SuccessAsync(1, "User Created.");
        }
    }
}
