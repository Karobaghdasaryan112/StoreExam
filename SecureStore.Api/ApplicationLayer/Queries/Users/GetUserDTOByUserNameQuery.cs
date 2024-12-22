using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.Users
{
    public class GetUserDTOByUserNameQuery : IRequest<UserDTO>
    {
        public string UserName { get; set; }    
    }
}
