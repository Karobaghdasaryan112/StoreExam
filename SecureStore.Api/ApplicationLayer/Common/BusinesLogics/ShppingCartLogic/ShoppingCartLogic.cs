using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApplicationLayer.Common.BusinesLogics.ShppingCartLogic
{
    public class ShoppingCartLogic
    {


        public static CartItem CreateCartItem
            (
            int productId,
            int quantity,
            decimal productPrice,
            int? shoppingCartId = null
            )
        {
            if (quantity <= 0 || productPrice <= 0)
                throw new ArgumentException("Invalid quantity or product price");

            return new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = quantity * productPrice,
                ShoppingCartId = shoppingCartId
            };

        }


        public static async Task cartItemOperation
            (
            Product product,
            CartItem cartItem,
            ShoppingCart shoppingCart,
            int Quantity,
            ICartItemRepository cartItemRepository
            )
        {

            if (cartItem != null)
            {
                cartItem.Quantity += cartItem.Quantity;
                cartItem.TotalPrice = cartItem.Quantity * product.Price;

                await cartItemRepository.UpdateCartItemAsync(cartItem);
            }
            else
            {
                cartItem = CreateCartItem(product.Id, Quantity, product.Price, shoppingCart.Id);

                shoppingCart.Items.Add(cartItem);

                await cartItemRepository.CreateCartItemAsync(cartItem);
            }
        }


        public static async Task CreaetingNewCartItem
            (
            Product product,
            CartItem cartItem,
            ShoppingCart shoppingCart,
            int userId,
            int Quantity,
            ICartItemRepository cartItemRepository,
            IShoppingCartRepository cardRepository
            )
        {
            cartItem = CreateCartItem(product.Id, Quantity, product.Price);
            shoppingCart = new ShoppingCart
            {
                UserId = userId,
                Items = new List<CartItem> { cartItem }
            };

            await cardRepository.CreateShoppingCartAsync(shoppingCart);

            cartItem.ShoppingCartId = shoppingCart.Id;

            await cartItemRepository.CreateCartItemAsync(cartItem);
        }


        public static void InvalidQauntityException(CartItem cartItem, int Quantity, int ProductQuantity)
        {

            if (cartItem?.Quantity + Quantity > ProductQuantity)

                throw new InvalidOperationException("invalid Quantity");
        }

    }
}
