using FluentValidation;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.EntitiesValidations
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(order => order.OrderDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Order date must be in the past or present.");

            RuleFor(order => order.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be positive.");

        }
    }

}
