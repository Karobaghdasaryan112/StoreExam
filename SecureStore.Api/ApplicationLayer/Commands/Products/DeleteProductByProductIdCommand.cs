using MediatR;

namespace SecureStore.Api.ApplicationLayer.Commands.Products
{
    public class DeleteProductByProductIdCommand : IRequest<bool>
    {
        public int ProductId {  get; set; }
    }
}
