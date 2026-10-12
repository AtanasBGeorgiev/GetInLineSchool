using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class MultiClassValidator : AbstractValidator<MultiClassRequest>
    {
        public MultiClassValidator()
        {
            RuleFor(m => m.CountYears)
                .NotEmpty().WithMessage("Count years is required!")
                .InclusiveBetween((byte)1, (byte)5).WithMessage("Count of years has to be between 1-5.");
            RuleFor(m => m.CountClasses)
                .NotEmpty().WithMessage("Count of classes is required.")
                .InclusiveBetween((byte)1, (byte)12).WithMessage("Count of classes has to be 1-12.");
            RuleFor(m => m.IDClassCategory)
                .NotEmpty().WithMessage("IDClassCategory is required!");
        }
    }
}
