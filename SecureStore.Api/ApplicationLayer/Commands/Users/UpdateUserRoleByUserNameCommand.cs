using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.Users
{
    public class UpdateUserRoleByUserNameCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public int AdminId { get; set; }
    }
}
