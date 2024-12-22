using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries
{
    public class GetUserDTOByEmailQuery : IRequest<UserDTO>
    {
        public string Email{  get; set; }
    }
}
