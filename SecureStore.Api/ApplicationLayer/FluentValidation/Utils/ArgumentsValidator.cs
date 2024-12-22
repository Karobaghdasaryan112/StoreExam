using System.Runtime.CompilerServices;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.Utils
{
    public class ArgumentsValidator
    {
        public void IsValidArguments(
            int arg1,
            int arg2,
            [CallerArgumentExpression("arg1")] string arg1Name = null,
            [CallerArgumentExpression("arg2")] string arg2Name = null)
        {
            if (arg1 < 1)
                throw new ArgumentException($"{arg1Name} must be greater than 0");

            if (arg2 < 1)
                throw new ArgumentException($"{arg2Name} must be greater than 0");
        }

        public void IsValidArguments(
            int arg1,
            [CallerArgumentExpression("arg1")] string arg1Name = null)
        {
            if (arg1 < 1)
                throw new ArgumentException($"{arg1Name} must be greater than 0");
        }
    }
}
