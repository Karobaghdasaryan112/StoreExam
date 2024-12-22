using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using System.Data;
using System.Transactions;


namespace SecureStore.Api.InfrastructureLayer.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private ApplicationDbContext _applicationDbContext;
        private DatabaseExceptionHandler _dbExceptionhandler;
        private TransactionException _transactionHendler;

        public ShoppingCartRepository
            (
            ApplicationDbContext context,
            DatabaseExceptionHandler handler
            )

        {
            _applicationDbContext = context;
            _dbExceptionhandler = handler;
        }



        public async Task<bool> CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            return await _dbExceptionhandler.HandleException(async () =>
            {
                await _applicationDbContext.ShoppingCarts.AddAsync(shoppingCart);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }


        public async Task<bool> DeleteShoppingCartAsync(int ShoppingCartId)
        {
            return await _dbExceptionhandler.HandleException(async () =>
            {
                var ShoppingCart = await _applicationDbContext.ShoppingCarts.FirstOrDefaultAsync(sh => sh.Id == ShoppingCartId);

                _applicationDbContext.ShoppingCarts.Remove(ShoppingCart);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }

        public async Task<ShoppingCart> GetShoppingCartByShoppingCartIdAsync(int ShoppingCartId)
        {
            return await _dbExceptionhandler.HandleException(async () =>
            {
                var ShoppingCart = await _applicationDbContext.ShoppingCarts.FirstOrDefaultAsync(SH => SH.Id == ShoppingCartId);

                return ShoppingCart;
            });
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(int UserId)
        {
            return await _dbExceptionhandler.HandleException(async () =>
            {
                var ShoppingCart = await _applicationDbContext.ShoppingCarts.Where(sh => sh.UserId == UserId).
                Include(sh => sh.User).
                Include(sh => sh.Items).
                ThenInclude(I => I.Product).
                FirstOrDefaultAsync();

                return ShoppingCart;
            });
        }


    }
}
