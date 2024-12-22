using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Queries.Products
{
    public class GetProductsByPageAndPageSizeQuery : IRequest<ICollection<ProductDTO>>
    {
        public int Page { get; set; }
        public int ProductCountInPage { get; set; }
    }
}
