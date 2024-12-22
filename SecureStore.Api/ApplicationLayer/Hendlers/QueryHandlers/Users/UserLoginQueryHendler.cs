using AutoMapper;
using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.Users;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.Utils;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users
{
    public class UserLoginQueryHendler : IRequestHandler<UserLoginQuery, string>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IUserRepository _userRepository;
        private IValidator<UserLoginQuery> _userLoginValidator;
        private JwtTokenHelper _jwtTokenHelper;
        private IMapper _mapper;
        public UserLoginQueryHendler(
            ValidationExceptionHandler validationExceptionHandler,
            IUserRepository userRepository,
            IValidator<UserLoginQuery> validator,
            IMapper mapper,
            JwtTokenHelper  jwtTokenHelper
            )
        {
            _validationExceptionHandler = validationExceptionHandler;
            _userRepository = userRepository;
            _userLoginValidator = validator;
            _mapper = mapper;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<string> Handle(UserLoginQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                await Ensure.EnsureValid(request, _userLoginValidator);

                var User = await _userRepository.GetUserByUserNameAsync(request.UserName);
                if (User != null)
                {
                    if (PasswordHelper.ValidatePassword(request.Password, User.PasswordHash))
                    {
                        var UserDTO = _mapper.Map<UserDTO>(User);
                        return _jwtTokenHelper.GenerateToken(UserDTO.Email, UserDTO.UserName, UserDTO.RoleName, UserDTO.Id);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid password provided.");
                    }
                }

                throw new InvalidOperationException($"User with username '{request.UserName}' not found.");
            });
        }

    }
}
