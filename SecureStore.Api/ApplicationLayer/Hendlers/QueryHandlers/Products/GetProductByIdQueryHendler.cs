using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.Extentions;
using SecureStore.Api.ApplicationLayer.FluentValidation.Utils;
using SecureStore.Api.ApplicationLayer.Queries.Products;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Products
{
    public class GetProductByIdQueryHendler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private IMapper _mapper;
        private ArgumentsValidator _argumentsValidator;
        private IValidator<Product> _validator;
        private IDistributedCache _distributedCache;
        public GetProductByIdQueryHendler(
            ValidationExceptionHandler validationExceptionHandler,
            IProductRepository productRepository, 
            IMapper mapper,
            IValidator<Product> validator,
            ArgumentsValidator argumentsValidator,
            IDistributedCache distributedCache)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = productRepository;
            _mapper = mapper;
            _argumentsValidator = argumentsValidator;
            _validator = validator;
            _distributedCache = distributedCache;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidator.IsValidArguments(request.ProductId);
                var cachekey = $"product:{request.ProductId}";

                var Product = await _distributedCache.GetOrSetAsync<Product>(cachekey, async () =>
                {
                    return await _productRepository.GetProductByIdAsync(request.ProductId);
                }, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(10) 
                });

                if (Product == null)
                    throw new InvalidOperationException($"Product with ID {request.ProductId} was not found.");

                return _mapper.Map<ProductDTO>(Product);

            });

        }
    }
}
