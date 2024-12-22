using MediatR;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts
{
    public class GetShoppingCartByUserIdQuery : IRequest<ShoppingCartDTO>
    {
        public int UserId {  get; set; }
    }
}
