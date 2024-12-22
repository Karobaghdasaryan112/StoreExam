using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.Utils;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Users
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand,UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidator;
        private IMapper _mapper;
        public EditUserCommandHandler(
            IUserRepository userRepository,
            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidator = argumentsValidator;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                var OldUser = await _userRepository.GetUserByIdAsync(request.UserId);


                if (string.IsNullOrEmpty(request.NewUserName) && string.IsNullOrEmpty(request.NewPassword) && string.IsNullOrEmpty(request.NewEmail))
                {
                    throw new ArgumentNullException("Invalid data: At least one field (UserName, Email, or Password) must be provided.");
                }


                var NewUser = new User()
                {
                    UserName = OldUser.UserName,
                    Email = OldUser.Email,
                    CreatedAt = OldUser.CreatedAt,
                    DateOfBirth = OldUser.DateOfBirth,
                    FirstName = OldUser.FirstName,
                    LastName = OldUser.LastName,
                    Id = OldUser.Id,
                    Orders = OldUser.Orders,
                    PasswordHash = OldUser.PasswordHash,
                    PhoneNumber = OldUser.PhoneNumber,
                    Role = OldUser.Role,
                    RoleId = OldUser.RoleId,
                    ShoppingCart = OldUser.ShoppingCart,
                };


                if (!string.IsNullOrEmpty(request.NewEmail))
                    NewUser.Email = request.NewEmail;

                if (!string.IsNullOrEmpty(request.NewPassword))
                {
                    NewUser.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
                }

                if (!string.IsNullOrEmpty(request.NewUserName))
                {

                    var existingUser = await _userRepository.GetUserByUserNameAsync(request.NewUserName);

                    if (existingUser != null)
                    {
                        throw new ArgumentException("User with this username already exists.");
                    }
                    NewUser.UserName = request.NewUserName;
                }


                var NewUserDTO = _mapper.Map<UserDTO>(NewUser);


                await _userRepository.UpdateUserAsync(OldUser, NewUser);

                return NewUserDTO;

            });
        }
    }
}
