using FluentValidation;
using SecureStore.Api.DomainLayer.Entities;

namespace SecureStore.Api.ApplicationLayer.FluentValidation.EntitiesValidations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("Password hash is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(user => user.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(user => user.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Age must be reasonable (less than 120 years old).");

            RuleFor(user => user.CreatedAt)
                .NotEmpty().WithMessage("Created date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Created date must be in the past or present.");

            RuleFor(user => user.UpdatedAt)
                .GreaterThanOrEqualTo(user => user.CreatedAt).WithMessage("Updated date cannot be earlier than the created date.");
        }
    }

}

