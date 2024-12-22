using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureStore.Api.DomainLayer.Entities;
using System.Reflection.Emit;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
            .Property(OI => OI.TotalAmount)
            .HasColumnType("decimal(18,2)");

            builder.HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId);

            builder.HasMany(order => order.Items)
                .WithOne(orderItem => orderItem.Order)
                .HasForeignKey(order => order.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
