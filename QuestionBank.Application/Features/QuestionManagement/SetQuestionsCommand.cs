using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Application.Features.QuestionManagement;

/// <summary>
/// Command to set the questions for a specific interview.
/// </summary>
public class SetQuestionsCommand : IRequest<bool>
{
    /// <summary>
    /// Interview ID to which the questions belong.
    /// </summary>
    /// <example>12</example>
    [JsonIgnore]
    public int InterviewId { get; set; }

    /// <summary>
    /// DTOs representing modifications to <see cref="InterviewQuestionDetail"/> entities.
    /// </summary>
    /// <example>
    /// [
    ///   { "id": 0, "question": "What is Dependency Injection?"},
    ///   { "id": 15, "question": ""},
    ///   { "id": 5, "question": "Explain SOLID principles."}
    /// ]
    /// </example>
    /// <seealso cref="QuestionUpdateDto"/>
    /// <seealso cref="InterviewQuestionDetail"/>
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
        var interview = await _dbContext.Interviews
                        .AsQueryable()
                        .Include(i => i.Questions)
                        .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null)
            throw new KeyNotFoundException($"Interview with ID {request.InterviewId} not found.");

        var existingQuestionsDict = interview.Questions.ToDictionary(q => q.Id);

        var newQuestions = request.Questions
            .Where(q => q.Id == 0)
            .ToList();
        var deletions = request.Questions
            .Where(q => q.Id > 0 && string.IsNullOrWhiteSpace(q.Question))
            .ToList();
        var updates = request.Questions
            .Where(q => q.Id > 0 && !string.IsNullOrWhiteSpace(q.Question))
            .ToList();

        // Validate deletions and updates reference valid existing questions
        var invalidIds = deletions.Concat(updates)
            .Where(q => !existingQuestionsDict.ContainsKey(q.Id))
            .Select(q => q.Id)
            .ToList();
        if (invalidIds.Any())
            throw new KeyNotFoundException($"Some question IDs not found: {string.Join(", ", invalidIds)}");

        // Normalize and collect new + updated questions for duplication check
        var newOrUpdatedQuestions = newQuestions
            .Concat(updates)
            .Select(q => q.Question!.Trim().ToLowerInvariant())
            .ToList();

        //Get Existing questions under the interview in a normalised format
        var existingQuestionsForInterview = interview.Questions
        .Select(q => q.Question.Trim().ToLowerInvariant()).ToHashSet();

        //Check for duplicate between questions in request and questions in DB
        var duplicate = newOrUpdatedQuestions
                        .FirstOrDefault(text => existingQuestionsForInterview
                        .Contains(text));

        if (duplicate is not null)
            throw new InvalidOperationException($"Duplicate question for this interview: '{duplicate}'");


        // Add new questions
        var entitiesToAdd = newQuestions.Select(q => new InterviewQuestionDetail
        {
            InterviewId = request.InterviewId,
            Question = q.Question!.Trim()
        }).ToList();
        _dbContext.InterviewQuestionDetails.AddRange(entitiesToAdd);

        // Remove deleted questions
        var entitiesToDelete = deletions.Select(q => existingQuestionsDict[q.Id]).ToList();
        _dbContext.InterviewQuestionDetails.RemoveRange(entitiesToDelete);

        // Update changed questions
        foreach (var update in updates)
        {
            var entity = existingQuestionsDict[update.Id];
            var newText = update.Question!.Trim();
            if (entity.Question != newText)
            {
                entity.Question = newText;
                _dbContext.InterviewQuestionDetails.Update(entity);
            }
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
