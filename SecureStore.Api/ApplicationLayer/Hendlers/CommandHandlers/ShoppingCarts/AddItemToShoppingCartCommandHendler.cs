using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Common.BusinesLogics.ShppingCartLogic;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.UnitOfWorks;
using System.Transactions;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.ShoppingCarts
{
    public class AddItemToShoppingCartCommandHendler : IRequestHandler<AddItemToShoppingCartCommand,ShoppingCartDTO>
    {
        private IShoppingCartRepository _cardRepository;
        private ICartItemRepository _cartItemRepository;
        private IProductRepository _productRepository;

        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidation;

        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ILogger<AddItemToShoppingCartCommandHendler> _logger; 

        public AddItemToShoppingCartCommandHendler(
            IShoppingCartRepository shoppingCartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository,
            ValidationExceptionHandler validationExceptionHandler,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddItemToShoppingCartCommandHendler> logger,
            ArgumentsValidator argumentsValidation
            )
        {
             _cardRepository = shoppingCartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;

            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ShoppingCartDTO> Handle(AddItemToShoppingCartCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.productId, request.quantity);

                var product = await _productRepository.GetProductByIdAsync(request.productId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID '{request.productId}' not found.");

                var shoppingCart = await _cardRepository.GetShoppingCartByUserIdAsync(request.UserId);

                CartItem cartItem;

                try
                {
                    using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    {
                        if (shoppingCart != null)
                        {
                            cartItem = shoppingCart.Items.FirstOrDefault(ci => ci.ProductId == request.productId);

                            try
                            {
                                ShoppingCartLogic.InvalidQauntityException(cartItem, request.quantity, product.StockQuantity);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Invalid quantity for product '{product.Name}' in shopping cart.", ex);
                            }

                            try
                            {
                                await ShoppingCartLogic.cartItemOperation(product, cartItem, shoppingCart, request.quantity, _cartItemRepository);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException($"Failed to update cart item for product '{product.Name}'.", ex);
                            }
                        }
                        else
                        {
                            cartItem = new CartItem();

                            await ShoppingCartLogic.CreaetingNewCartItem(
                                product,
                                cartItem,
                                shoppingCart,
                                request.UserId,
                                request.quantity,
                                _cartItemRepository,
                                _cardRepository
                            );

                        }

                        try
                        {
                            await _unitOfWork.CommitTransactionAsync();
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to commit the transaction while adding product to shopping cart.", ex);
                        }

                        return _mapper.Map<ShoppingCartDTO>(shoppingCart);
                    }
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();

                    _logger.LogError(ex.Message);

                    throw new TransactionException("An error occurred while processing the AddProductToShoppingCart request.", ex);
                }
            });
        }


    }
}
