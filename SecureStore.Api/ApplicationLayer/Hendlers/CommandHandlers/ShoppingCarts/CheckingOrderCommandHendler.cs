using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Common.BusinesLogics.OrderingLogic;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.UnitOfWorks;
using System.Transactions;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.ShoppingCarts
{
    public class CheckingOrderCommandHendler : IRequestHandler<CheckingOrderCommand,OrderDTO>
    {
        private IShoppingCartRepository _cardRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private ICartItemRepository _cartItemRepository;

        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidation;

        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ILogger<CheckingOrderCommandHendler> _logger;
        public CheckingOrderCommandHendler(
            IShoppingCartRepository cardRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICartItemRepository cartItemRepository,

            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidation,
            IUnitOfWork unitOfWork,

            IMapper mapper,
            ILogger<CheckingOrderCommandHendler> logger)
        {
            _cardRepository = cardRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;

            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;

            _unitOfWork = unitOfWork;

            _mapper = mapper;
            _logger = logger;
        }


        public async Task<OrderDTO> Handle(CheckingOrderCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.UserId);

                var ShoppingCart = await _cardRepository.GetShoppingCartByUserIdAsync(request.UserId);

                if (ShoppingCart is null || !ShoppingCart.Items.Any())
                {
                    throw new InvalidOperationException("The shopping cart is either empty or does not exist for the given user.");
                }

                var CartItems = ShoppingCart.Items;

                try
                {
                    using (var transaction = await _unitOfWork.BeginTransactionAsync())
                    {
                        try
                        {
                            await OrderCreatingLogic.ProductUpdateingOperation(CartItems, _productRepository);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to update product information during order processing.", ex);
                        }

                        Order NewOrder;
                        try
                        {

                            NewOrder = await OrderCreatingLogic.CreatingOrderOperation(
                                CartItems,
                                _orderRepository,
                                _cartItemRepository,
                                request.UserId
                            );

                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to create the order.", ex);
                        }

                        try
                        {
                            await _unitOfWork.CommitTransactionAsync();
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to commit the transaction.", ex);
                        }

                        return _mapper.Map<OrderDTO>(NewOrder);
                    }
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();

                    throw new TransactionException("An error occurred during the transaction, and it was rolled back.", ex);
                }
            });
        }



    }
}
