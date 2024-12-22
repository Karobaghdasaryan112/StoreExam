using FluentValidation;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.EntitiesValidations
{
    public class ShoppingCartValidator : AbstractValidator<ShoppingCart>
    {
        public ShoppingCartValidator()
        {
            RuleFor(cart => cart.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

        }
    }

}
