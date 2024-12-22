using FluentValidation;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.EntitiesValidations
{
    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(item => item.TotalPrice)
                .GreaterThan(0).WithMessage("Total price must be positive.");
        }
    }

}
