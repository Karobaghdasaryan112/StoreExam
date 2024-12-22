using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.Users
{
    public class GetUserDTOByIdQuery : IRequest<UserDTO>
    {
        public int UserId {  get; set; }
    }
}
