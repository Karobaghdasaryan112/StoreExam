using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Commands.Products;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.CommandHandlers.Products
{
    public class UpdateProductQuantityCommandHendler : IRequestHandler<UpdateProductQuantityCommand,ProductDTO>
    {
        private IProductRepository _productRepository;
        private IMapper _mapper;
        private IValidator<Product> _validator;
        private ValidationExceptionHandler _validationExceptionHandler;
        private ArgumentsValidator _argumentsValidation;
        private IDistributedCache _distributedCache;
        private IEntityOperationWithCaching<Product> _entityOperationWithCaching;
        public UpdateProductQuantityCommandHendler(
            IProductRepository productRepository, 
            IMapper mapper, 
            IValidator<Product> validator,
            ValidationExceptionHandler validationExceptionHandler,
            ArgumentsValidator argumentsValidation,
            IDistributedCache distributedCache,
            IEntityOperationWithCaching<Product> entityOperationWithCaching) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
            _validationExceptionHandler = validationExceptionHandler;
            _argumentsValidation = argumentsValidation;
            _distributedCache = distributedCache;
            _entityOperationWithCaching = entityOperationWithCaching;
        }

        public async Task<ProductDTO> Handle(UpdateProductQuantityCommand request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidation.IsValidArguments(request.Quantity, request.productId);

                var Product = await _productRepository.GetProductByIdAsync(request.productId);

                var CacheKey = $"Product:{request.productId}";

                if (Product == null)
                    throw new InvalidOperationException($"Product with ID '{request.productId}' not found.");

                var UpdatedPorduct = await _entityOperationWithCaching.PerformOperationWithCaching(
                    Product,
                    CacheKey,
                    _distributedCache,
                    async () => await _productRepository.UpdateProductQuantityAsync(request.productId, request.Quantity),
                    async () => await _productRepository.GetProductByIdAsync(request.productId));


                return _mapper.Map<ProductDTO>(UpdatedPorduct);
            });
        }


    }
}
