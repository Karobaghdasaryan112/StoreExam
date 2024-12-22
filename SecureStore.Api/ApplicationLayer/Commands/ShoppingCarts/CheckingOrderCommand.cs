using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts
{
    public class CheckingOrderCommand : IRequest<OrderDTO>
    {
        public int UserId;
    }
}
