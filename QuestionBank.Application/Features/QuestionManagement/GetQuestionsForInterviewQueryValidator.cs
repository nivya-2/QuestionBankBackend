using FluentValidation;
using QuestionBank.Application.Dto;


namespace QuestionBank.Application.Features.QuestionManagement;

/// <summary>
/// Validator for the <see cref="GetQuestionsForInterviewQuery"/> that
/// ensures a valid interview ID is provided.
/// </summary>
public class GetQuestionsForInterviewQueryValidator : AbstractValidator<GetQuestionsForInterviewQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetQuestionsForInterviewQueryValidator"/> class.
    /// Defines validation rules for the <see cref="GetQuestionsForInterviewQuery"/>.
    /// </summary>
    ///
    public GetQuestionsForInterviewQueryValidator()
    {
        // Ensure InterviewId is greater than 0
        RuleFor(x => x.InterviewId)
            .GreaterThan(0)
            .WithMessage("InterviewId must be greater than zero.");
    }
}
