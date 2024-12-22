using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Users
{
    public class DeleteUserByUserNameCommandHandler : IRequestHandler<DeleteUserByUserNameCommand, bool>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IUserRepository _userRepository;

        public DeleteUserByUserNameCommandHandler(
            ValidationExceptionHandler validationExceptionHandler,
            IUserRepository userRepository)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserByUserNameCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                if (request.UserName == null)
                {
                    throw new ArgumentNullException(nameof(request.UserName), "UserName cannot be null. Please provide a valid username.");
                }

                if (await _userRepository.DeleteUserByUserNameAsync(request.UserName))
                    return true;

                return false;
            });
        }
    }
}
