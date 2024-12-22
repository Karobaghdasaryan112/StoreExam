using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.Users;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users
{

    public class GetUserDTOByIdQueryHandler : IRequestHandler<GetUserDTOByIdQuery, UserDTO>
    {
        private IUserRepository _userRepository { get; set; }
        private ValidationExceptionHandler _validationExceptionHandler;
        private IMapper _mapper;
        private ArgumentsValidator _argumentsValidation;
        public GetUserDTOByIdQueryHandler(
            IUserRepository userRepository, 
            ValidationExceptionHandler validationExceptionHandler,
            IMapper mapper,
            ArgumentsValidator argumentsValidation
            )
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _mapper = mapper;
            _argumentsValidation = argumentsValidation;
        }
        public async Task<UserDTO> Handle(GetUserDTOByIdQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.UserId);

                var User = await _userRepository.GetUserByIdAsync(request.UserId);

                if (User == null)
                {
                    throw new InvalidOperationException($"User with ID '{request.UserId}' was not found.");
                }

                var UserDTO = _mapper.Map<UserDTO>(User);

                return UserDTO;
            });
        }

    }
}
