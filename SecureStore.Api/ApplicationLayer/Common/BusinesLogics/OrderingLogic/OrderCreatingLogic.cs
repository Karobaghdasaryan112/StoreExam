using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Common.BusinesLogics.OrderingLogic
{
    public class OrderCreatingLogic
    {
        private IDistributedCache _distributedCache;
        
        public OrderCreatingLogic(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public static async Task ProductUpdateingOperation(ICollection<CartItem> CartItems, IProductRepository productRepository)
        {
            foreach (var item in CartItems)
            {

                var Product = item.Product;

                if (Product is null || Product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Product {item.ProductId} is out of stock.");
                }

                Product.StockQuantity -= item.Quantity;



                if (Product.StockQuantity == 0)
                    await productRepository.DeleteProductByProductIdAsync(Product.Id);

                await productRepository.UpdateProductQuantityAsync(Product.Id, Product.StockQuantity);


            }


        }

        public static async Task<Order> CreatingOrderOperation(ICollection<CartItem> CartItems, IOrderRepository orderRepository, ICartItemRepository cartItemRepository, int UserId)
        {
            var NewOrder = new Order()
            {
                Items = CartItems.Select(CI => new OrderItem()
                {
                    ProductId = CI.ProductId,
                    Quantity = CI.Quantity,
                    TotalPrice = CI.TotalPrice
                }).ToList(),

                OrderDate = DateTime.Now,

                TotalAmount = CartItems.Select(CI => CI.TotalPrice).Sum(),

                UserId = UserId,
            };


            foreach (var item in CartItems)
            {
                await cartItemRepository.DeleteCartItemAsync(item.Id);
            }

            await orderRepository.CreateOrderAsync(NewOrder);

            return NewOrder;
        }
    }
}
