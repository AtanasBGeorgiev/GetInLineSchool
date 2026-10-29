using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class ActivateStudentValidator : AbstractValidator<AtivateStudentRequest>
    {
        public ActivateStudentValidator()
        {
            RuleFor(a => a.Username)
                .NotEmpty().WithMessage("Username is required.");
            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required.");
            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(a => a.SignUpCode)
                .NotEmpty().WithMessage("SignUpCode is required.");
        }
    }
}
