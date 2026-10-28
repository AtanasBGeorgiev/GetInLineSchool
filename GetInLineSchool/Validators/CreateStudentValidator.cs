using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentRequest>
    {
        public CreateStudentValidator()
        {
            RuleFor(s => s.Names)
                .NotEmpty().WithMessage("Names is required.");
            RuleForEach(s => s.Names)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches(@"^[АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЮЯ]{2}$")
                .WithMessage("Each name must contain exactly two uppercase Cyrillic letters, e.g. ВГ.");
            RuleFor(s => s.IDClass)
               .NotNull().WithMessage("IDClass is required.");
        }
    }
}
