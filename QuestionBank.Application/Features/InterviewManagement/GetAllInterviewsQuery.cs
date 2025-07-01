using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.InterviewManagement;

#region LLD: GetAllInterviewsQuery
/*
Overview:
---------
This query retrieves a complete list of all interview records from the database.
Each record is returned in a lightweight DTO (`InterviewsDto`) that includes:
- The interview ID.
- The job role/title associated with the interview.
- The current interview status (converted from enum to string).
- Metadata such as created date and creator.

Process Flow:
-------------
1. The client triggers this query via a GET request to `/api/interviews`.
2. MediatR calls the `Handle` method of `GetAllInterviewsQueryHandler`.
3. The handler queries the `Interviews` DbSet using Entity Framework Core.
4. A projection is performed using `.Select()` to directly map data into `InterviewsDto`:
   - Interview ID, Role, Status (converted to string), CreatedBy, CreatedOn.
5. The result is materialized using `.ToListAsync()` and returned.

Output:
-------
- If interviews exist:
   -> A list of `InterviewsDto` records is returned.
- If no interviews exist:
   -> An empty list is returned.

Design Considerations:
----------------------
- Read-only query, compliant with CQRS principles (no side effects).
- Projection is done at the database level for optimal performance.
- Enum `Status` is explicitly converted to string to improve client readability (e.g., Swagger).
- Asynchronous handling is used with proper cancellation support.
- DTO abstracts away domain internals; only client-relevant data is exposed.
*/
#endregion

/// <summary>
/// Represents a request to retrieve all interviews.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of all interviews from the system.
/// </remarks>
public class GetAllInterviewsQuery : IRequest<List<InterviewsDto>>
{
}

/// <summary>
/// Handles the <see cref="GetAllInterviewsQuery"/> to retrieve all interviews.
/// </summary>
public class GetAllInterviewsQueryHandler : IRequestHandler<GetAllInterviewsQuery, List<InterviewsDto>>
{
    private readonly IQuestionBankDbContext _questionBankDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllInterviewsQueryHandler"/> class.
    /// </summary>
    /// <param name="questionBankDbContext">The database context for accessing interview data.</param>
    public GetAllInterviewsQueryHandler(IQuestionBankDbContext questionBankDbContext)
    {
        _questionBankDbContext = questionBankDbContext;
    }

    /// <summary>
    /// Handles the retrieval of all interviews.
    /// </summary>
    /// <param name="request">The request object (empty in this case).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A list of <see cref="InterviewsDto"/> containing interview details.</returns>
    public async Task<List<InterviewsDto>> Handle(GetAllInterviewsQuery request, CancellationToken cancellationToken)
    {
        var interviews = await _questionBankDbContext.Interviews
            .Select( interview => new InterviewsDto()
            {
                Id = interview.Id,
                Role = interview.Role,
                Status = interview.Status.ToString(),
                CreatedBy = interview.CreatedBy,
                CreatedOn = interview.CreatedOn
            })
            .ToListAsync(cancellationToken);
        return interviews;
    }
}
