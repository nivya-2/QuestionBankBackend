using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuestionBank.Application.Features.InterviewManagement;

/// <summary>
/// Command to update an existing interview.
/// </summary>
public class UpdateInterviewCommand : IRequest<bool>
{
    /// <summary>
    /// The ID of the interview to update.
    /// </summary>
    public int InterviewId { get; set; }

    /// <summary>
    /// Updated role/title for the interview.
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Updated status for the interview.
    /// </summary>
    public string InterviewStatus { get; set; } = null!;

    /// <summary>
    /// Updated experience value.
    /// </summary>
    public float Experience { get; set; }

    /// <summary>
    /// List of skill IDs to associate with the interview.
    /// </summary>
    public List<int> SkillIds { get; set; } = new();
}

/// <summary>
/// Handles the <see cref="UpdateInterviewCommand"/> to perform an update operation
/// on an existing interview entity, including its associated InterviewSkills.
/// </summary>
public class UpdateInterviewCommandHandler : IRequestHandler<UpdateInterviewCommand, bool>
{
    /// <summary>
    /// EF Core database context for accessing and updating interview data.
    /// </summary>
    private readonly IQuestionBankDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInterviewCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">Database context used to access and update interview entities.</param>
    public UpdateInterviewCommandHandler(IQuestionBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the <see cref="UpdateInterviewCommand"/> by updating an existing interview's details
    /// and its associated skills in the database.
    /// </summary>
    /// <param name="request">Command containing updated interview details and skill IDs.</param>
    /// <param name="cancellationToken">Cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// Returns <c>true</c> if the interview was successfully updated; 
    /// <c>false</c> if the interview does not exist.
    /// </returns>
    public async Task<bool> Handle(UpdateInterviewCommand request, CancellationToken cancellationToken)
    {
        #region LLD: UpdateInterview
        //Requires: dbContext, InterviewId, Role, InterviewStatus, Experience, SkillIds
        // 1. Receive InterviewId and the updated data: Role, Experience, Status, SkillIds.
        // 2. Fetch the Interview entity including its current InterviewSkills.
        // 3. If the interview does not exist, return false.
        // 4. Update the interview’s Role, Experience, and Status (parse string to enum).
        // 5. Remove all existing InterviewSkill records linked to this interview.
        // 6. For each SkillId in the request, create a new InterviewSkill and add it to the interview.
        // 7. Save all changes to the database.
        // 8. Return true to indicate success.
        #endregion

        // Retrieve the interview along with its existing InterviewSkills.
        // If the interview does not exist, return false indicating failure.
        var interview = await _dbContext.Interviews
            .Include(i => i.InterviewSkills)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview == null)
            return false;

        // Update basic properties (Role, Status, Experience).
        // Status is parsed from string to enum, with fallback to existing value if invalid.
        interview.Role = request.Role;
        interview.Status = Enum.TryParse(request.InterviewStatus, out Shared.QuestionBankEnums.InterviewStatus status)
            ? status
            : interview.Status;
        interview.Experience = request.Experience;

        // Replace existing InterviewSkills with a new set based on SkillIds.
        // Old associations are deleted explicitly to avoid orphaned records or FK constraint issues.
        var existingSkills = _dbContext.InterviewSkills
            .Where(s => s.InterviewId == interview.Id);
        _dbContext.InterviewSkills.RemoveRange(existingSkills);

        foreach (var skillId in request.SkillIds)
        {
            interview.InterviewSkills.Add(new Domain.Entities.InterviewSkill
            {
                InterviewId = interview.Id,
                SkillId = skillId
            });
        }

        // Persist all changes to the database in a single atomic operation.
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Indicate successful update.
        return true;
    }
}
