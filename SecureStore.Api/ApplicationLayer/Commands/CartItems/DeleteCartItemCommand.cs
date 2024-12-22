using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.CartItems
{
    public class DeleteCartItemCommand : IRequest<bool>
    {
        public int CartItemId { get; set; }
    }
}
