using Microsoft.EntityFrameworkCore;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private DatabaseExceptionHandler _dbHandler;
        private ApplicationDbContext _applicationDbContext;



        public CartItemRepository
            (
            ApplicationDbContext applicationDbContext,
            DatabaseExceptionHandler databaseExceptionHandler
            )
        {
            _applicationDbContext = applicationDbContext;
            _dbHandler = databaseExceptionHandler;
        }
        public async Task<bool> CreateCartItemAsync(CartItem cartItem)
        {
            return await _dbHandler.HandleException(async () =>
            {
                await _applicationDbContext.CartItems.AddAsync(cartItem);
                return true;
            });
        }

        public async Task<bool> DeleteCartItemsByCartIdAsync(int ShoppingCartId)
        {
            return await _dbHandler.HandleException(async () =>
            {

                var itemsToDelete = _applicationDbContext.CartItems.Where(ci => ci.ShoppingCartId == ShoppingCartId);

                _applicationDbContext.CartItems.RemoveRange(itemsToDelete);

                await _applicationDbContext.SaveChangesAsync();

                return true;

            });
        }

        public async Task<bool> DeleteCartItemAsync(int CartItemId)
        {
            return await _dbHandler.HandleException(async () =>
            {
                var CartItem = await _applicationDbContext.CartItems.FirstOrDefaultAsync(CI => CI.Id == CartItemId);

                if (CartItem == null) return false;



                _applicationDbContext.CartItems.Remove(CartItem);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }

        public async Task<CartItem> GetCartItemByIdAsync(int CartItemId)
        {
            return await _dbHandler.HandleException(async () =>
            {

                var CartItem = await _applicationDbContext.CartItems.Where(CI => CI.Id == CartItemId).
                Include(CI => CI.ShoppingCart).
                Include(CI => CI.Product).
                FirstOrDefaultAsync();

                return CartItem;
            });
        }

        public async Task<ICollection<CartItem>> GetCartItemsByCartIdAsync(int ShoppingCartId)
        {
            return await _dbHandler.HandleException(async () =>
            {
                var CartItems = await _applicationDbContext.CartItems.
                Where(cartItem => cartItem.ShoppingCartId == ShoppingCartId).
                ToListAsync();

                return CartItems;
            });
        }

        public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
        {
            return await _dbHandler.HandleException(async () =>
            {
                _applicationDbContext.CartItems.Update(cartItem);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }


    }
}
