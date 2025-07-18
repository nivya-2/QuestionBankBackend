using FluentValidation;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Validates the <see cref="UpdateInterviewCommand"/> with the following rules:
/// <list type="bullet">
///   <item><description><c>InterviewId</c> must be greater than 0.</description></item>
///   <item><description><c>Role</c> must not be null, empty, or whitespace; max 100 characters.</description></item>
///   <item><description><c>Experience</c> must be a non-negative number.</description></item>
///   <item><description><c>InterviewStatus</c> must be a valid enum value.</description></item>
///   <item><description><c>SkillIds</c> must have positive values and must not contain duplicates.</description></item>
/// </list>
/// </summary>
public class UpdateInterviewCommandValidator : AbstractValidator<UpdateInterviewCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInterviewCommandValidator"/> class.
    /// Defines validation rules for the <see cref="UpdateInterviewCommand"/>.
    /// </summary>
    public UpdateInterviewCommandValidator()
    {
        // InterviewId should be positive
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

        // SkillIds should be all positive and should not contain duplicates
        RuleFor(cmd => cmd.SkillIds)
            .NotNull().WithMessage("SkillIds list is required.")
            .Must(skillIds => skillIds.All(id => id > 0))
                .WithMessage("Skill IDs must be positive integers.")
            .Must(skillIds => skillIds.Distinct().Count() == skillIds.Count)
                .WithMessage("Duplicate skill IDs are not allowed.");
    }
}
