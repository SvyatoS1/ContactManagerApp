using ContactManagerApp.DTOs;
using FluentValidation;

namespace ContactManagerApp.Validators
{
    public class ContactValidator : AbstractValidator<UpdateContactDto>
    {
        public ContactValidator()
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

            RuleFor(c => c.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
                .WithMessage("Phone number is invalid.");

            RuleFor(c => c.Salary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Salary must be positive.");

            RuleFor(c => c.Married)
                .NotNull();
        }
    }
}
