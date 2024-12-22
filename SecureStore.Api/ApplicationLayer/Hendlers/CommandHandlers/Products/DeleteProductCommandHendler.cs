using FluentValidation;
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
    public class DeleteProductCommandHendler  : IRequestHandler<DeleteProductCommand,bool>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private IValidator<Product> _validator;
        private IDistributedCache _distributedCache;
        private IEntityOperationWithCaching<Product> _deletingEntityOperationWithCaching;
        public DeleteProductCommandHendler(
            ValidationExceptionHandler validationExceptionHandler,
            IProductRepository repository,
            IValidator<Product> validator,
            IDistributedCache distributedCache,
            IEntityOperationWithCaching<Product> deletingEntityOperationWithCaching)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = repository;
            _distributedCache = distributedCache;
            _validator = validator;
            _distributedCache = distributedCache;
            _deletingEntityOperationWithCaching = deletingEntityOperationWithCaching;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                await Ensure.EnsureValid(request.product, _validator);
                
                var CacheKey = "Products";

                    return await _deletingEntityOperationWithCaching.PerformOperationWithCaching(
                        request.product,
                        CacheKey,
                        _distributedCache,
                       async () => await _productRepository.DeleteProductAsync(request.product),
                       async () => await _productRepository.GetAllProductsAsync());
                
            });
        }
    }
}
