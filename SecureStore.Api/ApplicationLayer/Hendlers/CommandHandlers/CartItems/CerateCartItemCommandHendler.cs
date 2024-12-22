using FluentValidation;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.CartItems;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.CartItems
{
    public class CerateCartItemCommandHendler : IRequestHandler<CreateCartItemCommand,bool>
    {
        private ICartItemRepository _cartItemRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IValidator<CartItem> _validator;
        public CerateCartItemCommandHendler(
            ICartItemRepository cartItemRepository,
            ValidationExceptionHandler validationExceptionHandler,
            IValidator<CartItem> validator)
        {
            _cartItemRepository = cartItemRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _validator = validator;
        }

        public async Task<bool> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {

                await Ensure.EnsureValid(request.cartItem, _validator);

                return await _cartItemRepository.CreateCartItemAsync(request.cartItem);

            });

        }
    }
}
