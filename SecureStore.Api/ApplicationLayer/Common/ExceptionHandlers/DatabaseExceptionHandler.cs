using Microsoft.EntityFrameworkCore;
using SecureStore.Api.DomainLayer.Exceptions;
using System.Data;

namespace SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers
{
    public class DatabaseExceptionHandler
    {
        private readonly ILogger<DatabaseExceptionHandler> _logger;

        public DatabaseExceptionHandler(ILogger<DatabaseExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task<T> HandleException<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "The database operation was canceled.");
                throw new DatabaseException("The database operation was canceled.", ex);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "A concurrency conflict occurred.");
                throw new DatabaseException("A concurrency conflict occurred during database access.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error.");
                throw new DatabaseException("An error occurred while updating the database.", ex);
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "Data access error.");
                throw new DatabaseException("An error occurred while accessing the data.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new DatabaseException("An unexpected error occurred.", ex);
            }
        }

    }
}
