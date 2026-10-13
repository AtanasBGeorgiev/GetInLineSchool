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
                .InclusiveBetween(_currentYear - 12, _currentYear).WithMessage("Year can not be in the future or more than 12 years ago.");
            RuleFor(s => s.Class)
                .NotEmpty().WithMessage("Class is required.")
                 .Must(c => c >= 'А' && c <= 'Я').WithMessage("Class has to be from А-Я.");
            RuleFor(s => s.IDClassCategory)
                .NotEmpty().WithMessage("IDClassCategory is required.");
        }
    }
}
