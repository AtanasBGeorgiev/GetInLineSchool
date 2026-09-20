using FluentValidation;
using GetInLineSchool.Models;

namespace GetInLineSchool.Validators
{ 
    public class SchoolValidator:AbstractValidator<School>
    {
        public SchoolValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Name is required.");
            RuleFor(s => s.Address)
                .NotEmpty().WithMessage("Address is required.");
            RuleFor(s => s.City)
                .NotEmpty().WithMessage("City is required.");
            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(s => s.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^0\d{9}$").WithMessage("Invalid phone number format.");
            RuleFor(s => s.UnifiedIdentificationCode)
                .NotEmpty().WithMessage("Unified Identification Code is required.");
            RuleFor(s => s.MateriallyResponsiblePerson)
                .NotEmpty().WithMessage("Materially Responsible Person is required.");
            RuleFor(s => s.SubscriptionPlan)
                .NotEmpty().WithMessage("Subscription Plan is required.");
            RuleFor(s => s.IsPaid)
                .NotNull().WithMessage("IsPaid is required.");
            RuleFor(s => s.EnableStudents)
                .NotNull().WithMessage("EnableStudents is required.");
            RuleFor(s => s.SchoolCode)
                .NotNull().WithMessage("SchoolCode is required.");
        }
    }
}