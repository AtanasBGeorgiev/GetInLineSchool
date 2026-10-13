using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class SingleClassValidator : AbstractValidator<SingleClassRequest>
    {
        private int _currentYear = DateTime.Today > new DateTime(DateTime.Today.Year, 6, 30)
            ? DateTime.Today.Year
            : DateTime.Today.Year - 1;

        public SingleClassValidator()
        {
            RuleFor(s => s.Year)
                .NotEmpty().WithMessage("Year is required.")
                .Must((request, year) =>
                {
                    int duration = request.IDClassCategory switch
                    {
                        1 => 4,
                        2 => 3,
                        3 => 5,
                        _ => 0
                    };

                    return year >= _currentYear - duration + 1
                        && year <= _currentYear;
                })
                .WithMessage($"Year can not be in the future or too far in the past.");
            RuleFor(s => s.Class)
                .NotEmpty().WithMessage("Class is required.")
                 .Must(c => c >= 'А' && c <= 'Я').WithMessage("Class has to be from А-Я.");
            RuleFor(s => s.IDClassCategory)
                .NotEmpty().WithMessage("IDClassCategory is required.")
                .InclusiveBetween((byte)1, (byte)3).WithMessage("Wrong id class.");
        }
    }
}
