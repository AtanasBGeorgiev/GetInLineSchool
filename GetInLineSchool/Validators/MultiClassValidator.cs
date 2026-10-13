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
                .Must((request, year) =>
                {
                    int duration = request.IDClassCategory switch
                    {
                        1 => 4,
                        2 => 3,
                        3 => 5,
                        _ => 0
                    };

                    return year <= duration;
                })
                .WithMessage("Count of years has to be 3/4/5 depending on choosen category.");
            RuleFor(m => m.CountClasses)
                .NotEmpty().WithMessage("Count of classes is required.")
                .InclusiveBetween((byte)1, (byte)12).WithMessage("Count of classes has to be 1-12.");
            RuleFor(m => m.IDClassCategory)
                .NotEmpty().WithMessage("IDClassCategory is required!");
        }
    }
}
