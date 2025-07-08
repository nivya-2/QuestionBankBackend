using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Query to retrieve an interview by its ID.
/// </summary>
public class GetInterviewByIdQuery : IRequest<InterviewDto>
{
    /// <summary>
    /// The ID of the interview to retrieve.
    /// </summary>
    /// <example>42</example>
    public int InterviewId { get; set; }
}

/// <summary>
/// Handles the <see cref="GetInterviewByIdQuery"/> to fetch a single interview from the database.
/// </summary>
public class GetInterviewByIdQueryHandler : IRequestHandler<GetInterviewByIdQuery, InterviewDto?>
{
    /// <summary>
    /// Database context used to query interview data.
    /// </summary>
    private readonly IQuestionBankDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInterviewByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">Database context injected via dependency injection.</param>
    public GetInterviewByIdQueryHandler(IQuestionBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the query to fetch a specific interview by ID along with its skill names.
    /// </summary>
    /// <param name="request">Query containing the interview ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>An interview DTO or null if not found.</returns>
    public async Task<InterviewDto?> Handle(GetInterviewByIdQuery request, CancellationToken cancellationToken)
    {
        #region LLD: GetInterviewByIdQuery
        //Requires: dbContext, InterviewId
        // 1. Receive the InterviewId through the request.
        // 2. Query the Interviews table with the given ID.
        // 3. Eager load the associated InterviewSkills and their Skill details.
        // 4. If the interview is found:
        //    - Map it into InterviewDto.
        //    - Return the DTO object
        // 5. If not found, throw KeyNotFoundException.
        #endregion

        var interview = await _dbContext.Interviews
            .Include(i => i.InterviewSkills)
                .ThenInclude(link => link.Skill)
            .Where(i => i.Id == request.InterviewId)
            .Select(i => new InterviewDto
            {
                Role = i.Role,
                InterviewStatus = i.Status.ToString(),
                Experience = i.Experience ?? 0m,
                InterviewSkills = i.InterviewSkills
                                    .Select(skill => skill.Skill.Name)
                                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (interview == null)
            throw new KeyNotFoundException($"Interview with ID {request.InterviewId} not found.");
        return interview;
    }
}
