using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using System.Text.Json.Serialization;

namespace SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts
{
    public class AddItemToShoppingCartCommand : IRequest<ShoppingCartDTO>
    {

        public int productId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public int quantity { get; set; }

    }
}
