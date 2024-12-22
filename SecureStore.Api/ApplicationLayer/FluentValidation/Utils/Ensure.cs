using FluentValidation;
using SecureStore.Api.DomainLayer.Exceptions;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.Utils
{
    public class Ensure
    {
        public static async Task EnsureValid<T>(T Request, IValidator<T> Validator)
        {
            var validationResult = await Validator.ValidateAsync(Request);
            if (!validationResult.IsValid)
            {
                var errorDetails = validationResult.Errors
                    .Select(e => $"{e.PropertyName} -- {e.ErrorMessage}");

                throw new ValidateException($"Invalid Data Of {nameof(Request)}. Details. {string.Join(", ", errorDetails)}");
            }
        }
    }
}
