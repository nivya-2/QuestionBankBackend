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
            // Only apply validation when adding a new question (Id == 0)
            question.When(q => q.Id == 0, () =>
            {
                // Ensure the question text is not null, empty, or whitespace
                question.RuleFor(q => q.Question)
                        .NotEmpty()
                        .WithMessage("New questions must not be empty.")
                        .MaximumLength(50)
                        .WithMessage("Question must not exceed 50 characters.");
            });

            //Ensure that update does not result in exceeeding 50 characters for question
            question.When(q => q.Id > 0, () =>
            {
                question.RuleFor(q => q.Question)
                        .MaximumLength(50).WithMessage("Question must not exceed 50 characters.")
                        .When(q => !string.IsNullOrWhiteSpace(q.Question));
            });
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
