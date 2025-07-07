using MediatR;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Domain.Entities;

/// <summary>
/// Command to set the questions for a specific interview.
/// </summary>
public class SetQuestionsCommand : IRequest<bool>
{
    /// <summary>
    /// Interview ID to which the questions belong.
    /// </summary>
    /// <example>12</example>
    public int InterviewId { get; set; }

    /// <summary>
    /// DTOs representing modifications to <see cref="InterviewQuestionDetails"/> entities.
    /// </summary>
    /// <example>
    /// [
    ///   { "id": 0, "question": "What is Dependency Injection?"},
    ///   { "id": 15, "question": ""},
    ///   { "id": 5, "question": "Explain SOLID principles."}
    /// ]
    /// </example>
    /// <seealso cref="QuestionUpdateDto"/>
    /// <seealso cref="InterviewQuestionDetails"/>
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
        // Requires: dbContext, InterviewId, List of QuestionUpdateDto (each with Id, Question)
        // 1. Receive InterviewId and a list of QuestionUpdateDto representing changes to be applied.
        // 2. Fetch the Interview entity along with its related Questions.
        // 3. If the interview does not exist, throw a KeyNotFoundException.
        // 4. For each QuestionUpdateDto in the list, determine the operation using Id and Question:
        //      - If Id == 0:
        //          - If Question is null/empty → throw ArgumentException (cannot insert blank question).
        //          - Else → treat as a new question and add to context.
        //      - If Id > 0:
        //          - If Question is null/empty:
        //              - Attempt to find the existing question by Id.
        //              - If not found → throw KeyNotFoundException.
        //              - Else → delete it from dbContext.
        //          - If Question is non-empty:
        //              - Attempt to find the existing question by Id.
        //              - If not found → throw KeyNotFoundException.
        //              - Else If new text differs from existing → update Question text.
        //              - Else → skip (no update needed).
        // 5. Save all changes to the database using dbContext.SaveChangesAsync.
        // 6. Return true to indicate the questions were successfully updated.
        #endregion
        return true;
    }
}
