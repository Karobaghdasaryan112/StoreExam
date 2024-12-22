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
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, ICollection<ProductDTO>>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private IMapper _mapper;
        private IDistributedCache _distributedCache;
        public GetProductsByCategoryQueryHandler(
            ValidationExceptionHandler validationExceptionHandler,
            IProductRepository productRepository,
            IMapper mapper,
            IDistributedCache distributedCache)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = productRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }


        public async Task<ICollection<ProductDTO>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _validationExceptionHandler.HandleException(async () =>
            {
                if (string.IsNullOrEmpty(request.Categry))
                    throw new ArgumentNullException("the CategoryName is Empty");


                var CacheKey = $"Product:{request.Categry}";
                return await _distributedCache.GetOrSetAsync(CacheKey, async () =>
                 {
                     var ProductsByCategory = await _productRepository.GetProductsByCategoryName(request.Categry);

                     if (ProductsByCategory == null)
                         throw new InvalidOperationException($"Products with Nameof {request.Categry} Are not Found");

                     var ProductsDTO = ProductsByCategory.Select(P => _mapper.Map<ProductDTO>(P)).ToList();

                     return ProductsDTO;
                 }, new DistributedCacheEntryOptions().
                 SetSlidingExpiration(TimeSpan.FromHours(1)).
                 SetAbsoluteExpiration(TimeSpan.FromDays(10)));

            });

        }

    }
}
