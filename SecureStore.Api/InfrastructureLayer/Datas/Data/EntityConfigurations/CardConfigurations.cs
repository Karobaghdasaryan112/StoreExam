using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using SecureStore.Api.DomainLayer.Entities;
using System.Reflection.Emit;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class CardConfigurations : IEntityTypeConfiguration<ShoppingCart>
    {

        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {



            //SettingDatas
            var cart1 = new ShoppingCart { Id = 1, UserId = 2 };
            var cart2 = new ShoppingCart { Id = 2, UserId = 3 };



            builder.HasData(cart1, cart2);




            //Entity Configuration
            builder.HasOne(cart => cart.User)
                .WithOne(user => user.ShoppingCart)
                .HasForeignKey<ShoppingCart>(cart => cart.UserId);

            builder.HasMany(cart => cart.Items)
                .WithOne(cartItem => cartItem.ShoppingCart)
                .HasForeignKey(cartItem => cartItem.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
