using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.Users;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.Utils;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users
{
    public class ValidUserQueryHendler : IRequestHandler<ValidUserQuery,bool>
    {
        public IUserRepository _userRepository;
        public ValidationExceptionHandler _validationExceptionHandler { get; set; }

        public IValidator<ValidUserQuery> _userLoginvalidator;

        public ValidUserQueryHendler(
            ValidationExceptionHandler validationExceptionHandler,
            IUserRepository userRepository,
            IValidator<ValidUserQuery> validator)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _userLoginvalidator = validator;
        }

        public async Task<bool> Handle(ValidUserQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                await Ensure.EnsureValid(request, _userLoginvalidator);

                var User = await _userRepository.GetUserByUserNameAsync(request.UserName);
                if (User != null)
                {
                    if (PasswordHelper.ValidatePassword(request.Password, User.PasswordHash))
                    {
                        return true;
                    }
                }
                return false;
            });
        }

    }
}
