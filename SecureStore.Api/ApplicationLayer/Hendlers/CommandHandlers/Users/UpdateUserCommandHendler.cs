using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Users
{
    public class UpdateUserCommandHendler : IRequestHandler<UpdateUserCommand,bool>
    {
        private IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IValidator<User> _userValidator;

        public UpdateUserCommandHendler(
            IUserRepository userRepository,
            ValidationExceptionHandler validationExceptionHandler,
            IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _userValidator = userValidator;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                await Ensure.EnsureValid(request.NewUser, _userValidator);

                await _userRepository.UpdateUserAsync(request.Olduser, request.NewUser);

                return true;
            });
        }
    }
}
