using Microsoft.EntityFrameworkCore;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private DatabaseExceptionHandler _dataBaseExceptionHandler;
        private ApplicationDbContext _applicationDbContext;

        public ProductRepository
            (
            DatabaseExceptionHandler databaseExceptionHandler,
            ApplicationDbContext applicationDbContext
            )
        {
            _dataBaseExceptionHandler = databaseExceptionHandler;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {

                _applicationDbContext.Products.Remove(product);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }

        public async Task<bool> DeleteProductByProductIdAsync(int productId)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {

                var Product = await GetProductByIdAsync(productId);

                if (Product == null) return false;

                _applicationDbContext.Products.Remove(Product);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }

        public async Task<bool> DeleteProductByProductNameAsync(string ProductName)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {
                var Product = await GetProductByNameAsync(ProductName);
                if (Product == null) return false;
                _applicationDbContext.Products.Remove(Product);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            });
        }

        public async Task<ICollection<Product>> GetAllProductsAsync()
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {
                return await _applicationDbContext.Products.ToListAsync();
            });
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {

                var Product = await _applicationDbContext.Products.FirstOrDefaultAsync(P => P.Id == productId);

                return Product ?? default;

            });
        }

        public async Task<Product> GetProductByNameAsync(string ProductName)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {
                var Product = await _applicationDbContext.Products.FirstOrDefaultAsync(P => P.Name == ProductName);
                return Product ?? default;
            });
        }

        public async Task<ICollection<Product>> GetProductsByCategoryName(string CategoryName)
        {

            return await _dataBaseExceptionHandler.HandleException(async () =>
            {
               return await _applicationDbContext.Products.Where(P => P.Name == CategoryName).ToListAsync();
            });

        }

        public async Task<ICollection<Product>> GetProductsByPageAndPageSize(int Page, int ProductCountInPage)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {

                int skipCount = (Page - 1) * ProductCountInPage;

               
                var productsInPage = await _applicationDbContext.Products
                    .Skip(skipCount) 
                    .Take(ProductCountInPage) 
                    .ToListAsync();

                return productsInPage;

            });
        }


        public async Task<Product> UpdateProductQuantityAsync(int ProductId, int Quantity)
        {
            return await _dataBaseExceptionHandler.HandleException(async () =>
            {
                var Product = await GetProductByIdAsync(ProductId);

                if (Product == null) return default;

                Product.StockQuantity = Quantity;


                _applicationDbContext.Products.Update(Product);

                await _applicationDbContext.SaveChangesAsync();

                return Product;

            });
        }
    }
}
