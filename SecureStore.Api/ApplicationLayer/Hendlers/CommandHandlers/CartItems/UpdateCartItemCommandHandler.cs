using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Commands.CartItems;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.CartItems
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand,bool>
    {
        private ICartItemRepository _cartItemRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IValidator<CartItem> _validator;
        private IEntityOperationWithCaching<CartItem> _entityOperationWithCaching;
        private IDistributedCache _distributedCache;
        public UpdateCartItemCommandHandler(
            ICartItemRepository cartItemRepository,
            ValidationExceptionHandler validationExceptionHandler,
            IValidator<CartItem> validator,
            IEntityOperationWithCaching<CartItem> entityOperationWithCaching,
            IDistributedCache distributedCache)
        {
            _cartItemRepository = cartItemRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _validator = validator;
            _entityOperationWithCaching = entityOperationWithCaching;
            _distributedCache = distributedCache;
        }

        public async Task<bool> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {

            return await _validationExceptionHandler.HandleException(async () =>
            {
                await Ensure.EnsureValid(request.cartItem, _validator);
                var CachaKey = $"CartItem:{request.cartItem.Id}";

                await _cartItemRepository.UpdateCartItemAsync(request.cartItem);

                return true;

            });

        }
    }
}
