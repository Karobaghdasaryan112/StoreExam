using SecureStore.Api.DomainLayer.Exceptions;


namespace SecureStore.Api.ApplicationLayer.Common.ExceptionHandlers
{
    public class ValidationExceptionHandler
    {
        private readonly ILogger<ValidationExceptionHandler> _logger;

        public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async Task<T> HandleException<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (ex is ValidateException || ex is ArgumentNullException || ex is InvalidOperationException || ex is ArgumentException)
            {
                _logger.LogError(ex, "Exception of type {ExceptionType} occurred: {Message}", ex.GetType().Name, ex.Message);
                throw new ValidateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {Message}", ex.Message);
                throw new ValidateException("An unexpected error occurred.", ex);
            }
        }
    }
}


