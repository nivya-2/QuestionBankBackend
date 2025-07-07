using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionBank.Application.Contracts.Persistence;
using QuestionBank.Application.Dto;
using QuestionBank.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuestionBank.Shared;
using static QuestionBank.Shared.QuestionBankEnums;

/// <summary>
/// Command to update an existing interview.
/// </summary>
public class UpdateInterviewCommand : IRequest<bool>
{
    /// <summary>
    /// The ID of the interview to update.
    /// </summary>
    /// <example>50</example>
    public int InterviewId { get; set; }

    /// <summary>
    /// Updated role/title for the interview.
    /// </summary>
    /// <example>Software Developer</example>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Updated status for the interview.
    /// </summary>
    /// <example>0</example>
    /// <remarks>
    /// Provide the numeric value of the enum InterviewStatus:
    /// 0 = Draft, 1 = Submitted
    /// </remarks>
    public InterviewStatus InterviewStatus { get; set; }

    /// <summary>
    /// Updated experience value.
    /// </summary>
    /// <example>2.5</example>
    public decimal Experience { get; set; }

    /// <summary>
    /// List of skill IDs to associate with the interview.
    /// </summary>
    /// <example>[1,3]</example>
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
        // Requires: dbContext, InterviewId, Role, InterviewStatus, Experience, SkillIds
        // 1. Receive InterviewId and the updated data: Role, Experience, Status, SkillIds.
        // 2. Fetch the Interview entity including its current InterviewSkills.
        // 3. If the interview does not exist, throw an exception (invalid InterviewId).
        // 4. Update the interview’s Role, Experience, and Status directly from enum.
        // 5. Determine which SkillIds are:
        //      - New (in request but not in DB): Add them as new InterviewSkills.
        //      - Deleted (in DB but not in request): Remove them from InterviewSkills.
        //      - Unchanged (present in both): Leave them untouched.
        // 6. Apply add/remove operations accordingly.
        // 7. Save all changes to the database.
        // 8. Return true to indicate success.
        #endregion

        // Retrieve the interview along with its existing InterviewSkills.
        // If the interview does not exist, throw an exception.
        var interview = await _dbContext.Interviews
            .AsQueryable()
            .Include(i => i.InterviewSkills)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview == null)
            throw new KeyNotFoundException($"Interview with ID {request.InterviewId} not found.");

        // Update basic properties (Role, Status, Experience).
        if (!Enum.IsDefined(typeof(InterviewStatus), request.InterviewStatus))
            throw new ArgumentException("Invalid interview status value.");
        interview.Status = request.InterviewStatus;
        interview.Role = request.Role;
        interview.Experience = request.Experience;

        // 1. Get current skill IDs from the Interview entity
        var existingSkillIds = interview.InterviewSkills.Select(x => x.SkillId).ToList();
        var requestedSkillIds = request.SkillIds.Distinct().ToList();

        // 2. Find SkillIds to remove and add
        var skillIdsToRemove = existingSkillIds.Except(requestedSkillIds).ToList();
        var skillIdsToAdd = requestedSkillIds.Except(existingSkillIds).ToList();

        // 3. Remove InterviewSkills no longer present in request
         var skillsToDelete = interview.InterviewSkills
        .Where(x => skillIdsToRemove.Contains(x.SkillId))
        .ToList();
         _dbContext.InterviewSkills.RemoveRange(skillsToDelete);

        // 4. Add new InterviewSkills for any new skill IDs
        var newSkills = skillIdsToAdd
            .Select(skillId => new InterviewSkill
            {
                InterviewId = interview.Id,
                SkillId = skillId
            })
            .ToList();
        interview.InterviewSkills.AddRange(newSkills);

        // Persist all changes to the database
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Indicate successful update
        return true;
    }
}
