using MediatR;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.CartItems;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.CartItems
{
    public class GetCartItemByIdQueryHandler : IRequestHandler<GetCartItemByIdQuery,CartItem>
    {
        private ICartItemRepository _cartItemRepository;
        private ArgumentsValidator _argumentsValidator;


        public GetCartItemByIdQueryHandler(ICartItemRepository cartItemRepository,ArgumentsValidator argumentsValidator)
        {
            _cartItemRepository = cartItemRepository;
            _argumentsValidator = argumentsValidator;
        }

        public async Task<CartItem> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            _argumentsValidator.IsValidArguments(request.CartItemId);


            var CartItem = await _cartItemRepository.GetCartItemByIdAsync(request.CartItemId);

            if (CartItem == null)
                throw new InvalidOperationException($"Cart item with ID '{request.CartItemId}' was not found.");

            return CartItem;
        }

    }
}
