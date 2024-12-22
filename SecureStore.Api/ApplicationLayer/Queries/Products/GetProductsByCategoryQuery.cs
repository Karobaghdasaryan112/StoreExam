using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Queries.Products
{
    public class GetProductsByCategoryQuery : IRequest<ICollection<ProductDTO>>
    {
        public string Categry {  get; set; }
    }
}
