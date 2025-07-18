using FluentValidation;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Validator for <see cref="UpdateInterviewCommand"/> to ensure incoming data is valid.
/// </summary>
public class UpdateInterviewCommandValidator : AbstractValidator<UpdateInterviewCommand>
{
    public UpdateInterviewCommandValidator()
    {
        // InterviewId should be positive (though it's ignored from body, this prevents incorrect usage)
        RuleFor(cmd => cmd.InterviewId)
            .GreaterThan(0).WithMessage("InterviewId must be a positive integer.");

        // Role should not be null, empty or whitespace
        RuleFor(cmd => cmd.Role)
            .NotEmpty().WithMessage("Role is required.")
            .MaximumLength(100).WithMessage("Role must not exceed 100 characters.");

        // Experience must be a non-negative number
        RuleFor(cmd => cmd.Experience)
            .GreaterThanOrEqualTo(0).WithMessage("Experience must be a non-negative number.");

        // InterviewStatus must be a defined enum value
        RuleFor(cmd => cmd.InterviewStatus)
            .IsInEnum().WithMessage("Invalid interview status.");

        // SkillIds should not be null
        RuleFor(cmd => cmd.SkillIds)
            .NotNull().WithMessage("SkillIds list is required.");

        // SkillIds should not contain duplicates
        RuleFor(cmd => cmd.SkillIds)
            .Must(skillIds => skillIds.Distinct().Count() == skillIds.Count)
            .WithMessage("Duplicate skill IDs are not allowed.");
    }
}
