using FluentValidation;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.QuestionManagement;

/// <summary>
/// Validator for the <see cref="SetQuestionsCommand"/> that ensures all new questions are valid
/// and the <c>InterviewId</c> is a positive integer.
/// </summary>
public class SetQuestionsCommandValidator : AbstractValidator<SetQuestionsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetQuestionsCommandValidator"/> class.
    /// Defines validation rules for the <see cref="SetQuestionsCommand"/>.
    /// </summary>
    public SetQuestionsCommandValidator()
    {
        // Validate each question in the list
        RuleForEach(x => x.Questions).ChildRules(question =>
        {
            question.RuleFor(q => q.Question)
                .NotEmpty().WithMessage("Question must not be empty.")
                .MaximumLength(50).WithMessage("Question must not exceed 50 characters.");
        });

        //Ensure that the request has distinct questions
        RuleFor(cmd => cmd.Questions)
            .Must(HaveNoDuplicateQuestions)
            .WithMessage(cmd =>
            {
                var dups = cmd.Questions
                    .Where(q => !string.IsNullOrWhiteSpace(q.Question))
                    .Select(q => q.Question!.Trim().ToLowerInvariant())
                    .GroupBy(q => q)
                    .Where(g => g.Count() > 1)
                    .Select(g => $"'{g.Key}'")
                    .ToList();
                return $"Duplicate questions in request: {string.Join(", ", dups)}";
            });

        // Ensure InterviewId is greater than 0
        RuleFor(x => x.InterviewId)
            .GreaterThan(0)
            .WithMessage("InterviewId must be greater than zero.");
    }

    /// <summary>
    /// Checks whether the provided list of <see cref="QuestionUpdateDto"/> contains duplicate questions.
    /// </summary>
    /// <param name="questions">The list of questions to validate.</param>
    /// <returns>
    /// <c>true</c> if the list contains no duplicate questions (ignoring case and whitespace); 
    /// otherwise, <c>false</c>.
    /// </returns>
    private bool HaveNoDuplicateQuestions(List<QuestionUpdateDto> questions)
    {
        var normalized = questions
            .Where(q => !string.IsNullOrWhiteSpace(q.Question))
            .Select(q => q.Question!.Trim().ToLowerInvariant());

        return normalized.GroupBy(q => q).All(g => g.Count() == 1);
    }
}
