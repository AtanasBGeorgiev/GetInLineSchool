using FluentValidation;
using GetInLineSchool.DTOs.Request;

namespace GetInLineSchool.Validators
{
    public class TeacherQualificationValidator : AbstractValidator<TeacherQualificationsRequest>
    {
        public TeacherQualificationValidator()
        {
            RuleFor(t => t.IDClassCategories)
                .NotEmpty().WithMessage("At least one category.")
                .Must(categories => categories.All(c => c >= 1 && c <= 3))
                .WithMessage("Each category must be between 1 and 3.");
        }
    }
}
