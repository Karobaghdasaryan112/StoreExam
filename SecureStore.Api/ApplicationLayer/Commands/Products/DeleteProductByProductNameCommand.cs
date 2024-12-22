using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.Products
{
    public class DeleteProductByProductNameCommand : IRequest<bool>
    {
        public string ProductName { get; set; } 
    }
}
