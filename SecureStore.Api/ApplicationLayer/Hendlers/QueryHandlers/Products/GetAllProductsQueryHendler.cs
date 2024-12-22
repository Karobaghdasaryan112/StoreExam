using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.ApplicationLayer.Common.Extentions;
using SecureStore.Api.ApplicationLayer.Queries.Products;
using SecureStore.Api.DomainLayer.DTOs;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Hendlers.QueryHandlers.Products
{
    public class GetAllProductsQueryHendler :IRequestHandler<GetAllProductsQuery,ICollection<ProductDTO>>
    {
        private IProductRepository _productRepository;
        private ValidationExceptionHandler _validationExceptionHandler;
        private IMapper _mapper;
        private IDistributedCache _distributedCache;
        public GetAllProductsQueryHendler
            (
            IProductRepository productRepository,
            ValidationExceptionHandler validationExceptionHandler,
            IMapper mapper,
            IDistributedCache distributedCache
            )
        {
            _productRepository = productRepository;
            _validationExceptionHandler = validationExceptionHandler;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<ICollection<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                var cachkey = "Products";

               return await  _distributedCache.GetOrSetAsync(cachkey,async () =>
                {
                    var Products = await _productRepository.GetAllProductsAsync();

                    var productDTOs = Products.Select(product => _mapper.Map<ProductDTO>(product)).ToList();

                    return productDTOs;
                },new DistributedCacheEntryOptions().
                SetSlidingExpiration(TimeSpan.FromDays(1)).
                SetAbsoluteExpiration(TimeSpan.FromDays(30)));

            });
        }
    }
}
