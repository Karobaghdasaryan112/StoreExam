using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.ShoppingCarts
{
    public class ClearShoppingCartByUserIdCommandHendler : IRequestHandler<ClearShoppingCartByUserIdCommand,ShoppingCartDTO>
    {
        private ICartItemRepository _cartItemRepository;

        private IShoppingCartRepository _cardRepository;

        private ValidationExceptionHandler _validationExceptionHandler;

        private ArgumentsValidator _argumentsValidation;

        private IMapper _mapper;

        public ClearShoppingCartByUserIdCommandHendler(
            ICartItemRepository cartItemRepository,
            IShoppingCartRepository cardRepository, 
            ValidationExceptionHandler validationExceptionHandler,
            IMapper mapper,
            ArgumentsValidator argumentsValidation)
        {
            _cartItemRepository = cartItemRepository;
            _cardRepository = cardRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;
            _mapper = mapper;
        }

        public async Task<ShoppingCartDTO> Handle(ClearShoppingCartByUserIdCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.UserId);

                var ShoppingCart = await _cardRepository.GetShoppingCartByUserIdAsync(request.UserId);

                if (ShoppingCart == null)
                    throw new InvalidOperationException($"Shopping cart for user with ID '{request.UserId}' was not found.");

                if (ShoppingCart.Items.Any())
                {

                    await _cartItemRepository.DeleteCartItemsByCartIdAsync(ShoppingCart.Id);

                    ShoppingCart.Items.Clear();
                }

                return _mapper.Map<ShoppingCartDTO>(ShoppingCart);
            });
        }

    }
}
