using FluentValidation;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Validator for <see cref="GetInterviewByIdQuery"/>.
/// </summary>
/// <remarks>
/// Validates the following rules:
/// <list type="bullet">
///   <item><description>InterviewId must be greater than zero.</description></item>
/// </list>
/// </remarks>
public class GetInterviewByIdQueryValidator : AbstractValidator<GetInterviewByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetInterviewByIdQueryValidator"/> class.
    /// </summary>
    public GetInterviewByIdQueryValidator()
    {
        RuleFor(q => q.InterviewId)
            .GreaterThan(0).WithMessage("Interview ID must be a positive integer.");
    }
}
