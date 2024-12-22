using FluentValidation;
using SecureStore.Api.ApplicationLayer.Queries.Users;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.QueriesValidator
{
    public class UserLoginQueryValidator : AbstractValidator<UserLoginQuery>
    {
        public UserLoginQueryValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
