using AutoMapper;
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
    public class GetProductsByPageAndPageSizeQueryHendler : IRequestHandler<GetProductsByPageAndPageSizeQuery, ICollection<ProductDTO>>
    {
        private ValidationExceptionHandler _validationExceptionHandler;
        private IProductRepository _productRepository;
        private IMapper _mapper;
        private ArgumentsValidator _argumentsValidator;
        private IDistributedCache _distributedCache;
        public GetProductsByPageAndPageSizeQueryHendler(
            ValidationExceptionHandler validationExceptionHandler,
            IProductRepository productRepository,
            IMapper mapper,
            ArgumentsValidator argumentsValidator,
            IDistributedCache distributedCache)
        {
            _validationExceptionHandler = validationExceptionHandler;
            _productRepository = productRepository;
            _mapper = mapper;
            _argumentsValidator = argumentsValidator;
            _distributedCache = distributedCache;
        }

        public async Task<ICollection<ProductDTO>> Handle(GetProductsByPageAndPageSizeQuery request, CancellationToken cancellationToken)
        {

            return await _validationExceptionHandler.HandleException(async () =>
            {
                _argumentsValidator.IsValidArguments(request.Page, request.ProductCountInPage);

                var CacheKey = "Products";
                ICollection<Product> ProductsInPage = new List<Product>();

                var TryGet = _distributedCache.TryGetValue(CacheKey, out ICollection<Product> Products);

                if (TryGet)
                {
                    int skipCount = (request.Page - 1) * request.ProductCountInPage;
                    ProductsInPage = Products.Skip(skipCount).Take(request.ProductCountInPage).ToList();
                }
                else
                {
                    ProductsInPage = await _productRepository.GetProductsByPageAndPageSize(request.Page, request.ProductCountInPage);
                }

                if (ProductsInPage == null)
                    throw new InvalidOperationException($"There is 0 Products in PageOf {request.Page}");

                var ProductsInPageDTO = new List<ProductDTO>(ProductsInPage.Count);

                ProductsInPageDTO = ProductsInPage
                                   .Where(item => item != null)
                                   .Select(item => _mapper.Map<ProductDTO>(item))
                                   .ToList();

                return ProductsInPageDTO;
            });

        }
    }
}
