using Microsoft.Extensions.Caching.Distributed;

namespace SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces
{
    public interface IEntityOperationWithCaching<TEntity>
    {
        Task<TOperationResult> PerformOperationWithCaching<TOperationResult, TGettingResult>(
            TEntity entity,
            string CacheKey,
            IDistributedCache distributedCache,
            Func<Task<TOperationResult>> operationFunc,
            Func<Task<TGettingResult>> gettingFunc);
    }
}
