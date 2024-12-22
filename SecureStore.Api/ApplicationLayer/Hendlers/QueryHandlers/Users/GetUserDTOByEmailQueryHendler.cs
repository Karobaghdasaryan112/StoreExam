using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Queries;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Users
{
    public class GetUserdtoByEmailQueryHendler :IRequestHandler<GetUserDTOByEmailQuery,UserDTO>
    {
        private IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IMapper _mapper;

        public GetUserdtoByEmailQueryHendler(IUserRepository userRepository, ValidationExceptionHandler validationExceptionHandler, IMapper mapper)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserDTOByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    throw new ArgumentException("Email cannot be null, empty, or whitespace.");

                var emailAttribute = new EmailAddressAttribute();

                if (!emailAttribute.IsValid(request.Email))
                    throw new ArgumentException($"Email '{request.Email}' is not a valid email address.");

                var User = await _userRepository.GetUserByEmailAsync(request.Email);

                if (User == null)
                    throw new InvalidOperationException($"User with email '{request.Email}' was not found.");

                var UserDTO = _mapper.Map<UserDTO>(User);

                return UserDTO;
            });
        }

    }
}
