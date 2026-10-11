using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class SignUpCodeValidator : AbstractValidator<SignUpCodeRequest>
    {
        public SignUpCodeValidator()
        {
            RuleFor(s => s.CodeType)
                .InclusiveBetween((byte)1, (byte)2)
                .WithMessage("Invalid code type.");
        }
    }
}
