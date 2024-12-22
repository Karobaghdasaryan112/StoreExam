using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Users
{
    public class UpdateUserRoleByUserNameCommandHandler : IRequestHandler<UpdateUserRoleByUserNameCommand, bool>
    {
        private IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidator;
        public UpdateUserRoleByUserNameCommandHandler(
            IUserRepository userRepository, 
            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidator)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidator = argumentsValidator;
        }

        public async Task<bool> Handle(UpdateUserRoleByUserNameCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidator.IsValidArguments(request.AdminId);

                if (string.IsNullOrEmpty(request.UserName))
                    throw new ArgumentException("the UserName is Empty");

                var User = await _userRepository.GetUserByUserNameAsync(request.UserName);

                var Admin = await _userRepository.GetUserByIdAsync(request.AdminId);

                if (User == null || Admin == null) return false;

                await _userRepository.UpdaeUserRoleAsync(User, "Admin");

                await _userRepository.UpdaeUserRoleAsync(Admin, "User");

                return true;
            });
        }
    }
}
