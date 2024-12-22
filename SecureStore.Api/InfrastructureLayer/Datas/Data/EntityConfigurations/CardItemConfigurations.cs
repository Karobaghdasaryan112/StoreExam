using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class CardItemConfigurations : IEntityTypeConfiguration<CartItem>
    {


        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            //SettingDatas
            var cartItem1 = new CartItem
            {
                Id = 1,
                ShoppingCartId = 1,
                ProductId = 1,
                Quantity = 1,
                TotalPrice = 1500m
            };

            var cartItem2 = new CartItem
            {
                Id = 2,
                ShoppingCartId = 1,
                ProductId = 3,
                Quantity = 2,
                TotalPrice = 400m
            };

            var cartItem3 = new CartItem
            {
                Id = 3,
                ShoppingCartId = 2,
                ProductId = 2,
                Quantity = 1,
                TotalPrice = 800m
            };


            builder.HasData(cartItem1, cartItem2, cartItem3);




            //Entity Configuration
            builder.HasOne(cartItem => cartItem.ShoppingCart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(cartItem => cartItem.ShoppingCartId);

            builder.HasOne(cartItem => cartItem.Product)
                .WithMany()
                .HasForeignKey(cartItem => cartItem.ProductId);

        }
    }
}
