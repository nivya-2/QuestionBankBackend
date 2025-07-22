using MediatR;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Query to retrieve all skills.
/// </summary>
public class GetAllSkillsQuery : IRequest<List<SkillsDto>>
{
}

/// <summary>
/// Handles <see cref="GetAllSkillsQuery"/> to fetch all skills.
/// </summary>
public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, List<SkillsDto>>
{
    /// <summary>
    /// Provides access to the database context for querying all skills.
    /// </summary>
    private readonly IQuestionBankDbContext _questionBankDbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="GetAllInterviewsQueryHandler"/>.
    /// </summary>
    /// <param name="questionBankDbContext">The database context for accessing skills.</param>
    public GetAllSkillsQueryHandler(IQuestionBankDbContext questionBankDbContext)
    {
        _questionBankDbContext = questionBankDbContext;
    }

    /// <summary>
    /// Handles the <see cref="GetAllSkillsQuery"/> to retrieve all skills.
    /// </summary>
    /// <param name="request">The request object (empty in this case)</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A list <see cref="SkillsDto"/> containing all skills.</returns>
    public async Task<List<SkillsDto>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        #region LLD: GetAllSkillsQuery
        // 1. Query all skills from the database.
        // 2. Project each skill to SkillsDto.
        // 3. Return the list of DTOs.
        #endregion
        return new List<SkillsDto>();
    }
}
