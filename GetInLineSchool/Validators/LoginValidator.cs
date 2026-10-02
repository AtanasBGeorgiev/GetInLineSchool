using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(lr => lr.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(lr => lr.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
