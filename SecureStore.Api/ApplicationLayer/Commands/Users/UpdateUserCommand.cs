using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Commands.Users
{
    public class UpdateUserCommand : IRequest<bool>
    {

        public User Olduser;

        public User NewUser;

    }
}
