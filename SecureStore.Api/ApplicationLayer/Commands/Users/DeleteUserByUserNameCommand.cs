using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.Users
{
    public class DeleteUserByUserNameCommand : IRequest<bool>
    {
        public string UserName { get; set; }

    }
}
