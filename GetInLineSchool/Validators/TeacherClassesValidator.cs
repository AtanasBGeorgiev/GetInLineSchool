using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class TeacherClassesValidator : AbstractValidator<TeacherClassesRequest>
    {
        public TeacherClassesValidator()
        {
            RuleFor(t => t.TeacherClasses)
                .NotEmpty().WithMessage("At least one teacher class is required.");
        }
    }
}
