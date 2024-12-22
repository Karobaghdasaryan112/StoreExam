using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.CartItems;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.CartItems
{
    public class DeleteCardItemCommandHendler : IRequestHandler<DeleteCartItemCommand,bool>
    {

        private ICartItemRepository _cartItemRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidation;
        public DeleteCardItemCommandHendler(
            ICartItemRepository cartItemRepository,
            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidation)
        {
            _cartItemRepository = cartItemRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;
        }

        public async Task<bool> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {

            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.CartItemId);

                return await _cartItemRepository.DeleteCartItemAsync(request.CartItemId);

            });

        }
    }
}
