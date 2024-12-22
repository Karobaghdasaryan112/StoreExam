using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Queries.Products
{
    public class GetAllProductsQuery : IRequest<ICollection<ProductDTO>>
    {

    }
}
