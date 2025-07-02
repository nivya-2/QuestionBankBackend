using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuestionBank.Application.Features.InterviewManagement;
/*
Overview:
---------
This query retrieves a single interview record from the database based on a given Interview ID.
The goal is to return a lightweight DTO (`InterviewDto`) that contains:
- The role name of the interview.
- The current interview status as a string.
- The candidate's experience in years (defaults to 0 if not available).
- A list of skill names associated with the interview.

Process Flow:
-------------
1. A request is received with a specific `InterviewId`.
2. The `Handle` method is invoked by MediatR with the request and a cancellation token.
3. The Entity Framework Core context (`_dbContext`) is used to query the `Interviews` table.
4. To fetch the related skills, `.Include()` is used to eagerly load the `InterviewSkills` navigation property.
   - `.ThenInclude()` is used to further load each associated `Skill` entity.
5. The query filters the interview records by the given `InterviewId`.
6. A projection is performed using `.Select()` to directly map the interview data into an `InterviewDto`:
   - Role is taken directly.
   - Status enum is converted to string.
   - Experience is checked for null; if null, 0 is used.
   - InterviewSkills are projected to a list of their skill names.
7. The query returns the first matching record or null if not found.

Output:
-------
- If a matching interview exists:
   -> A fully populated `InterviewDto` is returned.
- If no record matches the given ID:
   -> The result is `null`.

Design Considerations:
----------------------
- This is a read-only query with no side effects (CQRS-compliant).
- Eager loading is used to reduce round trips and avoid lazy loading issues.
- Projection is done at the database level for performance; only needed fields are selected.
- The handler is asynchronous and respects cancellation via `cancellationToken`.
*/
#endregion

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
