using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Repositories
{
    public interface IProductRepository
    {
        Task<bool> DeleteProductAsync(Product product);

        Task<bool> DeleteProductByProductIdAsync(int productId);

        Task<bool> DeleteProductByProductNameAsync(string ProductName);

        Task<Product> GetProductByIdAsync(int productId);

        Task<Product> GetProductByNameAsync(string ProductName);

        Task<ICollection<Product>> GetAllProductsAsync();

        Task<Product> UpdateProductQuantityAsync(int ProductId, int Quantity);

        Task<ICollection<Product>> GetProductsByPageAndPageSize(int Page, int PageSize);

        Task<ICollection<Product>> GetProductsByCategoryName(string CategoryName);
    }
}
