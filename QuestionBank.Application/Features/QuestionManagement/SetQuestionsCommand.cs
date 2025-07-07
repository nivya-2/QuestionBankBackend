using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Domain.Entities;
using static QuestionBank.Shared.QuestionBankEnums;

/// <summary>
/// Command to set the questions for a specific interview.
/// </summary>
public class SetQuestionsCommand : IRequest<bool>
{
    /// <summary>
    /// Interview ID to which the questions belong.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// List of question details to be processed.
    /// </summary>
    public List<QuestionUpdateDto> Questions { get; set; } = new();
}

/// <summary>
/// Handles <see cref="SetQuestionsCommand"/> to update, insert, or retain questions associated with a specific interview.
/// </summary>
public class SetQuestionsCommandHandler : IRequestHandler<SetQuestionsCommand, bool>
{
    /// <summary>
    /// EF Core database context for accessing and updating questions in DB.
    /// </summary>
    private readonly IQuestionBankDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="SetQuestionsCommandHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context used to access and update interview entities.</param>
    public SetQuestionsCommandHandler(IQuestionBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the <see cref="SetQuestionsCommand"/> to set interview questions (insert/update/ignore).
    /// </summary>
    /// <param name="request">SetQuestionsCommand containing InterviewId and question payloads.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns <c>true</c> if changes were successfully saved.</returns>
    public async Task<bool> Handle(SetQuestionsCommand request, CancellationToken cancellationToken)
    {
        #region LLD: SetQuestionsForInterview
        // Requires: dbContext, InterviewId, List of QuestionUpdateDto (each with Id, QuestionText, ChangeType)
        // 1. Receive InterviewId and a list of QuestionUpdateDto representing changes to be applied.
        // 2. Fetch the Interview entity along with its related Questions.
        // 3. If the interview does not exist, throw a KeyNotFoundException.
        // 4. For each QuestionUpdateDto, perform action based on ChangeType:
        //      - Add: If Id == 0 → create and add new Question with provided QuestionText.
        //          - If Id ≠ 0 for Add, throw ArgumentException.
        //      - Update: If Id > 0 → locate existing Question and update its QuestionText.
        //          - If Id == 0 for Update, throw ArgumentException.
        //          - If question not found, throw KeyNotFoundException.
        //      - Delete: If Id > 0 → locate existing Question and remove it from the context.
        //          - If Id == 0 for Delete, throw ArgumentException.
        //          - If question not found, throw KeyNotFoundException.
        // 5. Save all changes to the database using dbContext.SaveChangesAsync.
        // 6. Return true to indicate the questions were successfully updated.
        #endregion
        return true;
    }
}
