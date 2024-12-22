using MediatR;
using SecureStore.Api.ApplicationLayer.Queries.CartItems;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.CartItems
{
    public class GetCartItemsByCartIdQueryHendler : IRequestHandler<GetCartItemsByCartIdQuery,ICollection<CartItem>>
    {
        private ICartItemRepository _cartItemRepository;

        public GetCartItemsByCartIdQueryHendler(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ICollection<CartItem>> Handle(GetCartItemsByCartIdQuery request, CancellationToken cancellationToken)
        {
           ICollection<CartItem> CartItems =  await _cartItemRepository.GetCartItemsByCartIdAsync(request.ShoppingCartId);

            return CartItems;
        }
    }
}
