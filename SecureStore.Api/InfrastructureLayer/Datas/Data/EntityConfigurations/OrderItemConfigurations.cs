using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder
            .Property(OI => OI.TotalPrice)
            .HasColumnType("decimal(18,2)");

            builder.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId);

            builder.HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(cardItem => cardItem.ProductId);
        }
    }
}
