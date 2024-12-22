using Microsoft.EntityFrameworkCore.Storage;

namespace SecureStore.Api.InfrastructureLayer.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
