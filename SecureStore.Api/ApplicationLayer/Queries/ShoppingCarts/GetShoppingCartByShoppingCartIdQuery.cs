using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts
{
    public class GetShoppingCartByShoppingCartIdQuery : IRequest<ShoppingCartDTO>
    {
        public int ShoppingCartId { get; set; } 
    }
}
