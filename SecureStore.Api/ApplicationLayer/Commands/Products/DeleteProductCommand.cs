using MediatR;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.Commands.Products
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Product product { get; set; }
    }
}
