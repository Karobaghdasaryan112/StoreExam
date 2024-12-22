using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.Utils;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private JwtTokenHelper _jwtTokenHelper;
        private IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            ValidationExceptionHandler validationExceptionHandler,
            JwtTokenHelper jwtTokenHelper,
            IValidator<CreateUserCommand> validator)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _jwtTokenHelper = jwtTokenHelper;
            _validator = validator;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

                return await _validationExceptionHandler.HandleException(async () =>
                {
                    await Ensure.EnsureValid(request, _validator);

                    var User = await _userRepository.CreateUserAsync(request);

                    Role role = new Role();
                    if (User.UserName == "AdminAdmin" && request.Password == "AdminAdmin")
                    {
                        role.Name = "Admin";
                    }
                    else
                    {
                        role.Name = "User";
                    }

                        User.Role = role;
                    
                    return _jwtTokenHelper.GenerateToken(User.Email, User.UserName, User.Role.Name, User.Id);
                });

        }
    }
}
