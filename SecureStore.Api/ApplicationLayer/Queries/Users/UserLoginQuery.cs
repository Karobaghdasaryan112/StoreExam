using MediatR;

namespace SecureStore.Api.ApplicationLayer.Queries.Users
{
    public class UserLoginQuery : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
