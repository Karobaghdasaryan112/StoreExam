using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Commands.CartItems
{
    public class CreateCartItemCommand : IRequest<bool>
    {
        public CartItem cartItem {  get; set; }
    }
}
