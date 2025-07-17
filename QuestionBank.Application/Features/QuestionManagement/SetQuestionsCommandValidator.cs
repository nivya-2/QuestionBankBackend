using FluentValidation;
using QuestionBank.Application.Features.QuestionManagement;

namespace QuestionBank.Application.Features.QuestionManagement;
public class SetQuestionsCommandValidator : AbstractValidator<SetQuestionsCommand>
{
    public SetQuestionsCommandValidator()
    {
        RuleForEach(x => x.Questions).ChildRules(question =>
        {
            question.When(q => q.Id == 0, () =>
            {
                question.RuleFor(q => q.Question)
                        .NotEmpty().WithMessage("New questions must not be empty.");
            });
        });
        RuleFor(x => x.InterviewId).GreaterThan(0).WithMessage("InterviewId must be greater than zero.");
    }
}
