using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Queries.CartItems
{
    public class GetCartItemsByCartIdQuery : IRequest<ICollection<CartItem>>
    {
        public int ShoppingCartId {  get; set; }
    }
}
