using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.InterviewManagement;

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
