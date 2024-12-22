using SecureStore.Api.ApplicationLayer.Commands.CartItems;
using SecureStore.Api.ApplicationLayer.Queries.CartItems;
using SecureStore.Api.DomainLayer.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Repositories
{
    public interface ICartItemRepository
    {
        Task<bool> CreateCartItemAsync(CartItem cartItem);
        Task<bool> UpdateCartItemAsync(CartItem CartItem);
        Task<bool> DeleteCartItemAsync(int CartItemId);
        Task<CartItem> GetCartItemByIdAsync(int CartItemId);
        Task<ICollection<CartItem>> GetCartItemsByCartIdAsync(int ShoppingCartId);

        Task<bool> DeleteCartItemsByCartIdAsync(int ShoppingCartId);
    }
}
