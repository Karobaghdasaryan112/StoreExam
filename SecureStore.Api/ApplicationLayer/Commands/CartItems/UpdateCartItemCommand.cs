using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Commands.CartItems
{
    public class UpdateCartItemCommand : IRequest<bool>
    {
        public CartItem cartItem { get; set; }
    }
}
