using Microsoft.Extensions.Caching.Distributed;
using SecureStore.Api.ApplicationLayer.Common.Extentions;
using SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Interfaces;

namespace SecureStore.Api.ApplicationLayer.Common.OperationsWithCaching.Implementations
{
    public class EntityOperationWithCaching<TEntity> : IEntityOperationWithCaching<TEntity>
    {
        public async Task<TOperationResult> PerformOperationWithCaching<TOperationResult, TGettingResult>(
            TEntity entity,
            string CacheKey,
            IDistributedCache distributedCache,
            Func<Task<TOperationResult>> updatingFunc,
            Func<Task<TGettingResult>> gettingFunc)
        {

            TOperationResult Succsess = await updatingFunc();



            var TryGet = distributedCache.TryGetValue(CacheKey, out ICollection<TEntity> Entities);

            if (TryGet)
            {
                Entities.Remove(entity);
                await distributedCache.RemoveAsync(CacheKey);
            }


            await distributedCache.SetAsync(CacheKey, await gettingFunc());

            return Succsess;
        }
    }
}
