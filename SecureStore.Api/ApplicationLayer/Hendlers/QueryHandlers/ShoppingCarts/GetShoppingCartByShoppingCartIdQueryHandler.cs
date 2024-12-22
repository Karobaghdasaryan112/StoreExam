using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.ShoppingCarts
{
    public class GetShoppingCartByShoppingCartIdQueryHandler : IRequestHandler<GetShoppingCartByShoppingCartIdQuery,ShoppingCartDTO>
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidation;
        private IMapper _mapper;
        public GetShoppingCartByShoppingCartIdQueryHandler(
            IShoppingCartRepository shoppingCartRepository,
            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidation,
            IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;
            _mapper = mapper;
        }

        public async Task<ShoppingCartDTO> Handle(GetShoppingCartByShoppingCartIdQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {


                _argumentsValidation.IsValidArguments(request.ShoppingCartId);

                var ShoppingCart = await _shoppingCartRepository.GetShoppingCartByShoppingCartIdAsync(request.ShoppingCartId);

                if (ShoppingCart == null)
                    throw new InvalidOperationException($"ShoppingCart With Id {request.ShoppingCartId} Was not found in the repository. Please ensure the ID is correct.");

                return _mapper.Map<ShoppingCartDTO>(ShoppingCart);
            });

        }

    }
}
