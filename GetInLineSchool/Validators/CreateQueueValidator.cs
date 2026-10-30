using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class CreateQueueValidator : AbstractValidator<CreateQueueRequest>
    {
        public CreateQueueValidator()
        {
            RuleFor(x => x.IDClass)
                .GreaterThan(0)
                .When(x => x.IDClass.HasValue)
                .WithMessage("Invalid class ID.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Subject is required.")
                .MaximumLength(100)
                .WithMessage("Subject cannot exceed 100 characters.");

            RuleFor(x => x.Duration)
                .InclusiveBetween((short)1, (short)420)
                .WithMessage("Duration must be between 1 and 420 minutes.");
        }
    }
}
