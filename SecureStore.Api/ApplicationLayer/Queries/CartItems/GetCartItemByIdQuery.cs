using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.CartItems
{
    public class GetCartItemByIdQuery : IRequest<CartItem>
    {
        public int CartItemId {  get; set; }
    }
}
