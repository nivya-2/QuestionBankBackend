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
    /// <example>12</example>
    public int InterviewId { get; set; }

    /// <summary>
    /// DTOs representing modifications to <see cref="Question"/> entities.
    /// </summary>
    /// <example>
    /// [
    ///   { "id": 0, "question": "What is Dependency Injection?", "shouldDelete": false },
    ///   { "id": 15, "question": "", "shouldDelete": true },
    ///   { "id": 5, "questionText": "Explain SOLID principles.", "shouldDelete": false }
    /// ]
    /// </example>
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
        // Requires: dbContext, InterviewId, List of QuestionUpdateDto (each with Id, Question, ShouldDelete)
        // 1. Receive InterviewId and a list of QuestionUpdateDto representing changes to be applied.
        // 2. Fetch the Interview entity along with its related Questions.
        // 3. If the interview does not exist, throw a KeyNotFoundException.
        // 4. For each QuestionUpdateDto, apply logic based on ShouldDelete and Id:
        //      - If ShouldDelete == true:
        //          - If Id == 0 → throw ArgumentException (cannot delete a question that doesn't exist).
        //          - If Id > 0:
        //              - Attempt to locate existing Question with that Id.
        //              - If not found → throw KeyNotFoundException.
        //              - Else → remove it from dbContext.
        //      - If ShouldDelete == false:
        //          - If Id == 0 → treat as new question → create new Question entity and add.
        //          - If Id > 0:
        //              - Attempt to locate existing Question.
        //              - If not found → throw KeyNotFoundException.
        //              - Else → update the QuestionText.
        // 5. Save all changes to the database using dbContext.SaveChangesAsync.
        // 6. Return true to indicate the questions were successfully updated.
        #endregion
        return true;
    }
}
