using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.QuestionManagement;

/// <summary>
/// Query to retrieve all questions associated with a specific interview.
/// </summary>
public class GetQuestionsForInterviewQuery : IRequest<List<QuestionUpdateDto>>
{
    /// <summary>
    /// Interview ID for which questions are to be fetched.
    /// </summary>
    /// <example>12</example>
    public int InterviewId { get; set; }
}

/// <summary>
/// Handles <see cref="GetQuestionsForInterviewQuery"/> to fetch all questions for a specific interview
/// </summary>
public class GetQuestionsForInterviewQueryHandler : IRequestHandler<GetQuestionsForInterviewQuery, List<QuestionUpdateDto>>
{
    /// <summary>
    /// EF Core database context for accessing questions in DB.
    /// </summary>
    private readonly IQuestionBankDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="GetQuestionsForInterviewQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context used to access and query interview entities.</param>
    public GetQuestionsForInterviewQueryHandler(IQuestionBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the <see cref="GetQuestionsForInterviewQuery"/> to retrieve all questions for a given interview.
    /// </summary>
    /// <param name="request">Query containing the InterviewId to fetch questions for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of <see cref="QuestionUpdateDto"/> representing existing questions.</returns>
    public async Task<List<QuestionUpdateDto>> Handle(GetQuestionsForInterviewQuery request, CancellationToken cancellationToken)
    {
        #region LLD: GetQuestionsForInterview
        // Requires: dbContext, InterviewId passed via GetQuestionsQuery
        // Goal: Retrieve all existing questions for a specific interview in editable format

        // 1. Accept InterviewId via GetQuestionsQuery.
        // 2. Query the database to fetch the Interview entity including its Questions navigation property.
        // 3. If the interview is not found, throw KeyNotFoundException with appropriate message.
        // 4. From the Interview.Questions collection, project each InterviewQuestionDetail to a QuestionUpdateDto:
        //    - Id: the ID of the question
        //    - Question: the question text
        // 5. Return the list of QuestionUpdateDto.
        #endregion
        var interview = await _dbContext.Interviews
                        .AsQueryable()
                        .Include(i => i.Questions)
                        .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);
        if (interview is null)
            throw new KeyNotFoundException($"Interview with ID {request.InterviewId} not found.");
        var interviewQuestions = interview.Questions.Select(
            q => new QuestionUpdateDto() {
                Id =q.Id,
                Question = q.Question,
            }
            ).ToList();
        return interviewQuestions;
    }
}

