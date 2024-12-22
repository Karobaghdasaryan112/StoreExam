using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureStore.Api.DomainLayer.Entities;
using System.Reflection.Emit;

namespace SecureStore.Api.InfrastructureLayer.Datas.Data.EntityConfigurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            //SettingDatas
            var user1 = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword1",
                RoleId = 1,

            };

            var user2 = new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PasswordHash = "hashedpassword2",
                RoleId = 2,

            };

            var user3 = new User
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                PasswordHash = "hashedpassword3",
                RoleId = 2,

            };

            builder.HasData(user1, user2, user3);


            builder.HasOne(user => user.Role)
                .WithMany(role => role.Users).
                HasForeignKey(user => user.RoleId);

            builder.HasOne(user => user.ShoppingCart)
                .WithOne(shoppingCart => shoppingCart.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
