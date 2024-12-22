using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Commands.Products;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Products
{
    public class DeleteProductByProductNameCommandHandler : IRequestHandler<DeleteProductByProductNameCommand, bool>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private IDistributedCache _distributedCache;
        private IEntityOperationWithCaching<Product> _deletingEntityOperationWithCaching;
        public DeleteProductByProductNameCommandHandler(
            ValidationExceptionHandler validationExceptionHandler, 
            IProductRepository productRepository,
            IDistributedCache distributedCache,
            IEntityOperationWithCaching<Product> deletingEntityOperationWithCaching)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = productRepository;
            _distributedCache = distributedCache;
            _deletingEntityOperationWithCaching = deletingEntityOperationWithCaching;
        }

        public async Task<bool> Handle(DeleteProductByProductNameCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                if (string.IsNullOrEmpty(request.ProductName))
                    throw new ArgumentNullException("the ProductName is Empty");

                var Product = await _productRepository.GetProductByNameAsync(request.ProductName);
                var CacheKey = "Products";
                    return await _deletingEntityOperationWithCaching.PerformOperationWithCaching(
                         Product,
                         CacheKey,
                        _distributedCache,
                        async () => await _productRepository.DeleteProductByProductNameAsync(request.ProductName),
                        async () => (await _productRepository.GetAllProductsAsync()).ToList()
                        );
            });
        }
    }
}
