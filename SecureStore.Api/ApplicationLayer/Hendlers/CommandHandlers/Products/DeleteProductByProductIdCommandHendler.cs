using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Commands.Products;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Products
{
    public class DeleteProductByProductIdCommandHendler : IRequestHandler<DeleteProductByProductIdCommand,bool>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private ArgumentsValidator _argumentsValidation;
        private IDistributedCache _distributedCache;
        private IEntityOperationWithCaching<Product> _entityOperationWithCaching;
        public DeleteProductByProductIdCommandHendler(
            ValidationExceptionHandler validationExceptionHandler, 
            IProductRepository productRepository,
            ArgumentsValidator argumentsValidation,
            IDistributedCache distributedCache,
            IEntityOperationWithCaching<Product> entityOperationWithCaching)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = productRepository;
            _argumentsValidation = argumentsValidation;
            _distributedCache = distributedCache;
            _entityOperationWithCaching = entityOperationWithCaching;
        }

        public async Task<bool> Handle(DeleteProductByProductIdCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.ProductId);

                var CacheKey = "Products";
                var DeleteProduct = await _productRepository.GetProductByIdAsync(request.ProductId);

                return await _entityOperationWithCaching.PerformOperationWithCaching(
                    DeleteProduct,
                    CacheKey,
                    _distributedCache,
                    async () => await _productRepository.DeleteProductByProductIdAsync(request.ProductId),
                    async () => await _productRepository.GetAllProductsAsync());


            });
        }
    }
}
