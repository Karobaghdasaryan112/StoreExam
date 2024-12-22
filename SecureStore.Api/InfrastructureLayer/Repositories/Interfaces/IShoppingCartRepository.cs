using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts;
using SecureStore.Api.DomainLayer.Entities;
using System.Data;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<bool> CreateShoppingCartAsync(ShoppingCart ShoppingCart);
        Task<ShoppingCart> GetShoppingCartByUserIdAsync(int UserId);
        Task<bool> DeleteShoppingCartAsync(int ShoppingCartId);
        Task<ShoppingCart> GetShoppingCartByShoppingCartIdAsync(int ShoppingCartId);
    }

}
