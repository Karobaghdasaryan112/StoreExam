using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrderAsync(Order order);

    }
}
