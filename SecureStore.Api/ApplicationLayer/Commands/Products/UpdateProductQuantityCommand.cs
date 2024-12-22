using MediatR;
using SecureStore.Api.DomainLayer.DTOs;

namespace SecureStore.Api.ApplicationLayer.Commands.Products
{
    public class UpdateProductQuantityCommand : IRequest<ProductDTO>
    {

        public int productId;

        public int Quantity;

    }
}
