using AutoMapper;
using MediatR;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.ShoppingCarts
{
    public class GetShoppingCartByUserIdQueryHandler : IRequestHandler<GetShoppingCartByUserIdQuery, ShoppingCartDTO>
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private ArgumentsValidator _argumentsValidator;
        private IMapper _mapper;

        public GetShoppingCartByUserIdQueryHandler(
            IShoppingCartRepository shoppingCartRepository,
            ArgumentsValidator argumentsValidator,
            IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _argumentsValidator = argumentsValidator;
            _mapper = mapper;
        }

        public async Task<ShoppingCartDTO> Handle(GetShoppingCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            _argumentsValidator.IsValidArguments(request.UserId);

            var ShoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(request.UserId);

            if (ShoppingCart == null)
                throw new InvalidOperationException($"Shopping cart for user with ID '{request.UserId}' was not found.");

            var ShoppingCartDTO = _mapper.Map<ShoppingCartDTO>(ShoppingCart);

            return ShoppingCartDTO;
        }

    }
}
