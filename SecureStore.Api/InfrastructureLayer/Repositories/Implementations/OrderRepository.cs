using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers;
using SecureStore.Api.DomainLayer.Entities;
using SecureStore.Api.InfrastructureLayer.Datas.Data;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.InfrastructureLayer.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext _context;
        private DatabaseExceptionHandler _databaseExceptionHandler;
        private IMapper _mapper;

        public OrderRepository
            (
            ApplicationDbContext context,
            DatabaseExceptionHandler databaseExceptionHandler,
            IMapper mapper
            )
        {
            _context = context;
            _databaseExceptionHandler = databaseExceptionHandler;
            _mapper = mapper;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            return await _databaseExceptionHandler.HandleException(async () =>
            {
                await _context.Orders.AddAsync(order);

                await _context.SaveChangesAsync();

                return true;
            });
        }


    }
}
