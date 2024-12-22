using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.CartItems
{
    public class DeleteCartItemsByCartIdCommand : IRequest<bool>
    {
        public int ShoppingCartId { get; set; }
    }
}
