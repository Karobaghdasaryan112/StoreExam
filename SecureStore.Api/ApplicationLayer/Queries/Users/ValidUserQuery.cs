using MediatR;

namespace SecureStore.Api.ApplicationLayer.Queries.Users
{
    public class ValidUserQuery : IRequest<bool>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

    }
}
