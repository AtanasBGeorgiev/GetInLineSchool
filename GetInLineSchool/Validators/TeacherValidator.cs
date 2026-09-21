using FluentValidation;
using GetInLineSchool.Models;

namespace GetInLineSchool.Validators
{
    public class TeacherValidator : AbstractValidator<Teacher>
    {
        public TeacherValidator()
        {
            RuleFor(t => t.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[А-ЯЁ]{2}\d{1,2}$")
                .WithMessage("Username must have the format АБ1 or АБ10.");
            RuleFor(t => t.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$")
                .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character. Lenght 8+ symbols.");
            RuleFor(t => t.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(t => t.IDSchool)
                .NotEmpty().WithMessage("ID is required.");
            RuleFor(t => t.Role)
                .NotEmpty().WithMessage("Role is required.")
                .InclusiveBetween(2,4).WithMessage("Invalid role value.");
        }
    }
}
