using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using System.Text.Json.Serialization;

namespace SecureStore.Api.ApplicationLayer.Commands.Users
{
    public class EditUserCommand : IRequest<UserDTO>
    {
        [JsonIgnore]
        public int UserId {  get; set; }
        public string NewUserName { get; set; } = null;
        public string NewPassword { get; set; } = null;
        public string NewEmail { get; set; } = null;
    }
}
