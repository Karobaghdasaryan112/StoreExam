using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;
using SecureStore.Api.InfrastructureLayer.Utils;
namespace SecureStore.Api.InfrastructureLayer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DatabaseExceptionHandler _databaseExceptionHandler;
        private IMapper _mapper;

        public UserRepository(ApplicationDbContext context,
            DatabaseExceptionHandler databaseException,
            IMapper mapper)
        {
            _context = context;
            _databaseExceptionHandler = databaseException;
            _mapper = mapper;
        }

        public async Task<User> CreateUserAsync(CreateUserCommand request)  
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                var newUser = _mapper.Map<User>(request);
                newUser.PasswordHash = PasswordHelper.HashPassword(request.Password);
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            });

        }

        public async Task<bool> DeleteUserByUserNameAsync(string UserName)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
               var User = await _context.Users.FirstOrDefaultAsync(U => U.UserName == UserName);

                if (User != null)
                {
                    _context.Users.Remove(User);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            });
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
                return user ?? null;

            });
        }

        public async Task<User> GetUserByIdAsync(int UserId)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                var user = await _context.Users.
                Where(U => U.Id == UserId).
                Include(U => U.ShoppingCart).
                FirstOrDefaultAsync();

                return user ?? null;
            });
        }

        public async Task<User> GetUserByUserNameAsync(string UserName)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                var user = await _context.Users.
                Where(u => u.UserName == UserName).
                Include(U => U.Role).
                Include(U => U.Orders).
                FirstOrDefaultAsync();

                return user ?? null;
            });
        }

        public async Task<bool> UpdateUserAsync(User oldUser, User newUser)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(U => U.UserName == oldUser.UserName);
                if (user != null)
                {
                    user.Email = newUser.Email;
                    user.UserName = newUser.UserName;
                    user.PasswordHash = newUser.PasswordHash;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            });
        }

        public async Task<bool> UpdaeUserRoleAsync(User user,string RoleName)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                user.Role.Name = RoleName;
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                return true;
            });
        }


    }
}
