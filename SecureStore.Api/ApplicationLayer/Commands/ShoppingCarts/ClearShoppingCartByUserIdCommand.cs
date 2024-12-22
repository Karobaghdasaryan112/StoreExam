using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts
{
    public class ClearShoppingCartByUserIdCommand : IRequest<ShoppingCartDTO>
    {
       public int UserId;
    }
}
