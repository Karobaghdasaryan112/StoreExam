using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class ProductConfigurastions : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            //SettingDatas
            var product1 = new Product { Id = 1, Name = "Laptop", Description = "High-end laptop", Price = 1500m };
            var product2 = new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 800m };
            var product3 = new Product { Id = 3, Name = "Headphones", Description = "Wireless headphones", Price = 200m };


            builder.HasData(product1, product2, product3);

            builder.HasMany(product => product.Categories)
                .WithMany(category => category.Products);

        }
    }
}
