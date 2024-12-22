using AutoMapper;
using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.Users;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users
{
    public class GetUserDTOByUserNameQueryHandler : IRequestHandler<GetUserDTOByUserNameQuery, UserDTO>
    {
        private IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IMapper _mapper;
        private IValidator<User> _validator;
        public GetUserDTOByUserNameQueryHandler(
            IUserRepository userRepository, 
            ValidationExceptionHandler validationExceptionHandler,
            IMapper mapper,
            IValidator<User> validator)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UserDTO> Handle(GetUserDTOByUserNameQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                if (string.IsNullOrWhiteSpace(request.UserName))
                    throw new ArgumentNullException(nameof(request.UserName), "Username cannot be null or empty.");
                

                var User = await _userRepository.GetUserByUserNameAsync(request.UserName);

                if (User == null)
                {
                    throw new InvalidOperationException($"User with username '{request.UserName}' was not found.");
                }

                var UserDTO = _mapper.Map<UserDTO>(User);

                return UserDTO;

            });
        }

    }
}
