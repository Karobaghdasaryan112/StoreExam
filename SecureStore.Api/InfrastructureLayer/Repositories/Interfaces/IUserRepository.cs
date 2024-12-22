using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Repositories
{
    public interface IUserRepository
    {
        //Create
        Task<User> CreateUserAsync(CreateUserCommand request);
        Task<bool> UpdaeUserRoleAsync(User user, string RoleName);



        //Read
        Task<User> GetUserByIdAsync(int UserId);
        Task<User> GetUserByUserNameAsync(string UserName);
        Task<User> GetUserByEmailAsync(string Email);




        //Update
        Task<bool> UpdateUserAsync(User Olduser, User NewUser);

        Task<bool> DeleteUserByUserNameAsync(string UserName);


    }
}
